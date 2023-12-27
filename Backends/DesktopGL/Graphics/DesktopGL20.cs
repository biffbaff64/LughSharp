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

namespace LibGDXSharp.Backends.Desktop;

public class DesktopGL20 : IGL20
{
    private ByteBuffer?  _buffer      = null;
    private FloatBuffer? _floatBuffer = null;
    private IntBuffer?   _intBuffer   = null;

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
        EnsureBufferCapacity( count << 2 );

        MemberNullException.ThrowIfNull( _floatBuffer );

        _floatBuffer.Clear();
        _floatBuffer.Limit = count;
        _floatBuffer.Put( v, offset, count );
        _floatBuffer.Position = 0;

        return _floatBuffer;
    }

    private IntBuffer ToIntBuffer( int[] v, int offset, int count )
    {
        EnsureBufferCapacity( count << 2 );

        MemberNullException.ThrowIfNull( _intBuffer );

        _intBuffer.Clear();
        _intBuffer.Limit = count;
        _intBuffer.Put( v, offset, count );
        _intBuffer.Position = 0;

        return _intBuffer;
    }

//    public void GLActiveTexture( int texture )
//    {
//        OpenGL.Gl.ActiveTexture( texture );
//    }

//    public void GLAttachShader( int program, int shader )
//    {
//        GL20.AttachShader( program, shader );
//    }

//    public void GLBindAttribLocation( int program, int index, String name )
//    {
//        Gl.BindAttribLocation( program, index, name );
//    }

//    public void GLBindBuffer( int target, int buffer )
//    {
//        Gl.BindBuffer( target, buffer );
//    }

//    public void GLBindFramebuffer( int target, int framebuffer )
//    {
//        EXTFramebufferObject.glBindFramebufferEXT( target, framebuffer );
//    }

//    public void GLBindRenderbuffer( int target, int renderbuffer )
//    {
//        EXTFramebufferObject.glBindRenderbufferEXT( target, renderbuffer );
//    }

//    public void glBindTexture( int target, int texture )
//    {
//        Gl.BindTexture( target, texture );
//    }

//    public void glBlendColor( float red, float green, float blue, float alpha )
//    {
//        GL14.glBlendColor( red, green, blue, alpha );
//    }

//    public void glBlendEquation( int mode )
//    {
//        GL14.glBlendEquation( mode );
//    }

//    public void glBlendEquationSeparate( int modeRGB, int modeAlpha )
//    {
//        Gl.BlendEquationSeparate( modeRGB, modeAlpha );
//    }

//    public void glBlendFunc( int sfactor, int dfactor )
//    {
//        Gl.BlendFunc( sfactor, dfactor );
//    }

//    public void glBlendFuncSeparate( int srcRGB, int dstRGB, int srcAlpha, int dstAlpha )
//    {
//        GL14.glBlendFuncSeparate( srcRGB, dstRGB, srcAlpha, dstAlpha );
//    }

    public void GLBufferData( OpenGL.BufferTarget target,
                              uint size,
                              Buffer? data,
                              OpenGL.BufferUsage usage )
    {
        if ( data == null )
        {
            Gl.BufferData( target, size, data, usage );
        }
        else if ( data is ByteBuffer byteBuffer )
        {
            Gl.BufferData( target, size, byteBuffer, usage );
        }
        else if ( data is IntBuffer intBuffer )
        {
            Gl.BufferData( target, size, intBuffer, usage );
        }
        else if ( data is FloatBuffer floatBuffer )
        {
            Gl.BufferData( target, size, floatBuffer, usage );
        }
        else if ( data is DoubleBuffer doubleBuffer )
        {
            Gl.BufferData( target, size, doubleBuffer, usage );
        }
        else if ( data is ShortBuffer shortBuffer )
        {
            Gl.BufferData( target, size, shortBuffer, usage );
        }
    }

    public void GLBufferSubData( OpenGL.BufferTarget target, IntPtr offset, uint size, Buffer data )
    {
        ArgumentNullException.ThrowIfNull( data );

        if ( data is ByteBuffer byteBuffer )
        {
            Gl.BufferSubData( target, offset, size, byteBuffer );
        }
        else if ( data is IntBuffer intBuffer )
        {
            Gl.BufferSubData( target, offset, intBuffer );
        }
        else if ( data is FloatBuffer floatBuffer )
        {
            Gl.BufferSubData( target, offset, floatBuffer );
        }
        else if ( data is DoubleBuffer )
        {
            Gl.BufferSubData( target, offset, ( DoubleBuffer )data );
        }
        else if ( data is ShortBuffer ) //
        {
            Gl.BufferSubData( target, offset, ( ShortBuffer )data );
        }
    }

    public int GLCheckFramebufferStatus( int target )
    {
        return EXTFramebufferObject.glCheckFramebufferStatusEXT( target );
    }

    public void GLClear( ClearBufferMask mask )
    {
        Gl.Clear( mask );
    }

    public void GLClearColor( float red, float green, float blue, float alpha )
    {
        Gl.ClearColor( red, green, blue, alpha );
    }

    public void GLClearDepthf( float depth )
    {
        Gl.ClearDepth( depth );
    }

    public void GLClearStencil( int s )
    {
        Gl.ClearStencil( s );
    }

    public void GLColorMask( bool red, bool green, bool blue, bool alpha )
    {
        Gl.ColorMask( red, green, blue, alpha );
    }

    public void GLCompileShader( uint shader )
    {
        Gl.CompileShader( shader );
    }

    public void GLCompressedTexImage2D( int target,
                                        int level,
                                        int internalformat,
                                        int width,
                                        int height,
                                        int border,
                                        int imageSize,
                                        Buffer data )
    {
        if ( data is ByteBuffer buffer )
        {
            Gl.CompressedTexImage2D( target, level, internalformat, width, height, border, buffer );
        }
        else
        {
            throw new GdxRuntimeException( $"Can't use {data.GetType().Name} with this method. Use ByteBuffer instead." );
        }
    }

    public void GLCompressedTexSubImage2D( int target,
                                           int level,
                                           int xoffset,
                                           int yoffset,
                                           int width,
                                           int height,
                                           int format,
                                           int imageSize,
                                           Buffer data )
    {
        throw new GdxRuntimeException( "not implemented" );
    }

    public void GLCopyTexImage2D( TextureTarget target,
                                  int level,
                                  OpenGL.InternalFormat internalformat,
                                  int x,
                                  int y,
                                  int width,
                                  int height,
                                  int border )
    {
        Gl.CopyTexImage2D( target, level, internalformat, x, y, width, height, border );
    }

    public void GLCopyTexSubImage2D( TextureTarget target, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
        Gl.CopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );
    }

    public uint GLCreateProgram()
    {
        return Gl.CreateProgram();
    }

    public uint GLCreateShader( ShaderType type )
    {
        return Gl.CreateShader( type );
    }

    public void GLCullFace( CullFaceMode mode )
    {
        Gl.CullFace( mode );
    }

    public void GLDeleteBuffers( int n, uint buffers )
    {
        Gl.DeleteBuffers( buffers );
    }

    public void GLDeleteBuffer( uint buffer )
    {
        Gl.DeleteBuffers( buffer );
    }

    public void GLDeleteFramebuffers( int n, IntBuffer framebuffers )
    {
        EXTFramebufferObject.glDeleteFramebuffersEXT( framebuffers );
    }

    public void GLDeleteFramebuffer( int framebuffer )
    {
        EXTFramebufferObject.glDeleteFramebuffersEXT( framebuffer );
    }

    public void GLDeleteProgram( uint program )
    {
        Gl.DeleteProgram( program );
    }

    public void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers )
    {
        EXTFramebufferObject.glDeleteRenderbuffersEXT( renderbuffers );
    }

    public void GLDeleteRenderbuffer( int renderbuffer )
    {
        EXTFramebufferObject.glDeleteRenderbuffersEXT( renderbuffer );
    }

    public void GLDeleteShader( uint shader )
    {
        Gl.DeleteShader( shader );
    }

    public void GLDeleteTextures( int n, uint textures )
    {
        Gl.DeleteTextures( textures );
    }

    public void GLDeleteTexture( uint texture )
    {
        Gl.DeleteTextures( texture );
    }

    public void GLDepthFunc( DepthFunction func )
    {
        Gl.DepthFunc( func );
    }

    public void GLDepthMask( bool flag )
    {
        Gl.DepthMask( flag );
    }

    public void GLDepthRangef( float zNear, float zFar )
    {
        Gl.DepthRange( zNear, zFar );
    }

    public void GLDetachShader( uint program, uint shader )
    {
        Gl.DetachShader( program, shader );
    }

    public void GLDisable( EnableCap cap )
    {
        Gl.Disable( cap );
    }

    public void GLDisableVertexAttribArray( uint index )
    {
        Gl.DisableVertexAttribArray( index );
    }

    public void GLDrawArrays( PrimitiveType mode, int first, int count )
    {
        Gl.DrawArrays( mode, first, count );
    }

    public void GLDrawElements( int mode, int count, int type, Buffer indices )
    {
        if ( indices is ShortBuffer buffer && ( type == IGL20.GL_UNSIGNED_SHORT ) )
        {
            Gl.DrawElements( mode, buffer );
        }
        else if ( indices is ByteBuffer byteBuf1 && ( type == IGL20.GL_UNSIGNED_SHORT ) )
        {
            Gl.DrawElements( mode, count, byteBuf1.AsShortBuffer() );
        }
        else if ( indices is ByteBuffer byteBuf2 && ( type == IGL20.GL_UNSIGNED_BYTE ) )
        {
            Gl.DrawElements( mode, count, byteBuf2 );
        }
        else

        {
            throw new GdxRuntimeException( $"Can't use {indices.GetType().Name} with this method. Use ShortBuffer or ByteBuffer instead." );
        }
    }

    public void GLEnable( EnableCap cap )
    {
        Gl.Enable( cap );
    }

    public void GLEnableVertexAttribArray( uint index )
    {
        Gl.EnableVertexAttribArray( index );
    }

    public void GLFinish()
    {
        Gl.Finish();
    }

    public void GLFlush()
    {
        Gl.Flush();
    }

    public void GLFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, int renderbuffer )
    {
        EXTFramebufferObject.glFramebufferRenderbufferEXT( target, attachment, renderbuffertarget, renderbuffer );
    }

    public void GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level )
    {
        EXTFramebufferObject.glFramebufferTexture2DEXT( target, attachment, textarget, texture, level );
    }

    public void GLFrontFace( FrontFaceDirection mode )
    {
        Gl.FrontFace( mode );
    }

    public void GLGenBuffers( int n, uint[] buffers )
    {
        Gl.GenBuffers( buffers );
    }

//    public uint GLGenBuffer()
//    {
//        return Gl.GenBuffer();
//    }

    public void GLGenFramebuffers( int n, IntBuffer framebuffers )
    {
        EXTFramebufferObject.glGenFramebuffersEXT( framebuffers );
    }

    public int GLGenFramebuffer()
    {
        return EXTFramebufferObject.glGenFramebuffersEXT();
    }

    public void GLGenRenderbuffers( int n, IntBuffer renderbuffers )
    {
        EXTFramebufferObject.glGenRenderbuffersEXT( renderbuffers );
    }

    public int GLGenRenderbuffer()
    {
        return EXTFramebufferObject.glGenRenderbuffersEXT();
    }

    public void GLGenTextures( int n, uint[] textures )
    {
        Gl.GenTextures( textures );
    }

    public uint GLGenTexture()
    {
        return Gl.GenTexture();
    }

    public void GLGenerateMipmap( int target )
    {
        EXTFramebufferObject.glGenerateMipmapEXT( target );
    }

    public String GLGetActiveAttrib( int program, int index, IntBuffer size, IntBuffer type )
    {
        return Gl.GetActiveAttrib( program, index, 256, size, type );
    }

    public String GLGetActiveUniform( int program, int index, IntBuffer size, IntBuffer type )
    {
        return Gl.GetActiveUniform( program, index, 256, size, type );
    }

    public void GLGetAttachedShaders( int program, int maxcount, Buffer count, IntBuffer shaders )
    {
        Gl.GetAttachedShaders( program, ( IntBuffer )count, shaders );
    }

    public int GLGetAttribLocation( int program, String name )
    {
        return Gl.GetAttribLocation( program, name );
    }

    public void GLGetBooleanv( int pname, Buffer parameters )
    {
        Gl.GetBooleanv( pname, ( ByteBuffer )parameters );
    }

    public void GLGetBufferParameteriv( int target, int pname, IntBuffer parameters )
    {
        Gl.GetBufferParameteriv( target, pname, parameters );
    }

    public ErrorCode GLGetError()
    {
        return Gl.GetError();
    }

    public void GLGetFloatv( int pname, FloatBuffer parameters )
    {
        Gl.GetFloatv( pname, parameters );
    }

    public void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters )
    {
        EXTFramebufferObject.glGetFramebufferAttachmentParameterivEXT( target, attachment, pname, parameters );
    }

    public void GLGetIntegerv( int pname, IntBuffer parameters )
    {
        Gl.GetIntegerv( pname, parameters );
    }

    public String GLGetProgramInfoLog( int program )
    {
        ByteBuffer buffer = ByteBuffer.allocateDirect( 1024 * 10 );
        buffer.order( ByteOrder.nativeOrder() );
        ByteBuffer tmp = ByteBuffer.allocateDirect( 4 );
        tmp.order( ByteOrder.nativeOrder() );
        IntBuffer intBuffer = tmp.asIntBuffer();

        Gl.GetProgramInfoLog( program, intBuffer, buffer );
        int    numBytes = intBuffer.get( 0 );
        byte[] bytes    = new byte[ numBytes ];
        buffer.get( bytes );

        return new String( bytes );
    }

    public void GLGetProgramiv( int program, int pname, IntBuffer parameters )
    {
        Gl.GetProgramiv( program, pname, parameters );
    }

    public void GLGetRenderbufferParameteriv( int target, int pname, IntBuffer parameters )
    {
        EXTFramebufferObject.glGetRenderbufferParameterivEXT( target, pname, parameters );
    }

    public String GLGetShaderInfoLog( int shader )
    {
        ByteBuffer buffer = ByteBuffer.allocateDirect( 1024 * 10 );
        buffer.order( ByteOrder.nativeOrder() );
        ByteBuffer tmp = ByteBuffer.allocateDirect( 4 );
        tmp.order( ByteOrder.nativeOrder() );
        IntBuffer intBuffer = tmp.asIntBuffer();

        Gl.GetShaderInfoLog( shader, intBuffer, buffer );
        int    numBytes = intBuffer.get( 0 );
        byte[] bytes    = new byte[ numBytes ];
        buffer.get( bytes );

        return new String( bytes );
    }

    public void GLGetShaderPrecisionFormat( int shadertype, int precisiontype, IntBuffer range, IntBuffer precision )
    {
        throw new UnsupportedOperationException( "unsupported, won't implement" );
    }

    public void GLGetShaderiv( int shader, int pname, IntBuffer parameters )
    {
        Gl.GetShaderiv( shader, pname, parameters );
    }

    public String GLGetString( int name )
    {
        return Gl.GetString( name );
    }

    public void GLGetTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
        Gl.GetTexParameterfv( target, pname, parameters );
    }

    public void GLGetTexParameteriv( int target, int pname, IntBuffer parameters )
    {
        Gl.GetTexParameteriv( target, pname, parameters );
    }

    public int GLGetUniformLocation( int program, String name )
    {
        return Gl.GetUniformLocation( program, name );
    }

    public void GLGetUniformfv( int program, int location, FloatBuffer parameters )
    {
        Gl.GetUniformfv( program, location, parameters );
    }

    public void GLGetUniformiv( int program, int location, IntBuffer parameters )
    {
        Gl.GetUniformiv( program, location, parameters );
    }

    public void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer )
    {
        throw new UnsupportedOperationException( "unsupported, won't implement" );
    }

    public void GLGetVertexAttribfv( int index, int pname, FloatBuffer parameters )
    {
        Gl.GetVertexAttribfv( index, pname, parameters );
    }

    public void GLGetVertexAttribiv( int index, int pname, IntBuffer parameters )
    {
        Gl.GetVertexAttribiv( index, pname, parameters );
    }

    public void GLHint( int target, int mode )
    {
        Gl.Hint( target, mode );
    }

    public bool GLIsBuffer( int buffer )
    {
        return Gl.IsBuffer( buffer );
    }

    public bool GLIsEnabled( int cap )
    {
        return Gl.IsEnabled( cap );
    }

    public bool GLIsFramebuffer( int framebuffer )
    {
        return EXTFramebufferObject.glIsFramebufferEXT( framebuffer );
    }

    public bool GLIsProgram( int program )
    {
        return Gl.IsProgram( program );
    }

    public bool GLIsRenderbuffer( int renderbuffer )
    {
        return EXTFramebufferObject.glIsRenderbufferEXT( renderbuffer );
    }

    public bool GLIsShader( int shader )
    {
        return Gl.IsShader( shader );
    }

    public bool GLIsTexture( int texture )
    {
        return Gl.IsTexture( texture );
    }

    public void GLLineWidth( float width )
    {
        Gl.LineWidth( width );
    }

    public void GLLinkProgram( int program )
    {
        Gl.LinkProgram( program );
    }

    public void GLPixelStorei( int pname, int param )
    {
        Gl.PixelStorei( pname, param );
    }

    public void GLPolygonOffset( float factor, float units )
    {
        Gl.PolygonOffset( factor, units );
    }

    public void GLReadPixels( int x, int y, int width, int height, int format, int type, Buffer pixels )
    {
        if ( pixels is ByteBuffer )
            Gl.ReadPixels( x, y, width, height, format, type, ( ByteBuffer )pixels );
        else if ( pixels is ShortBuffer )
            Gl.ReadPixels( x, y, width, height, format, type, ( ShortBuffer )pixels );
        else if ( pixels is IntBuffer )
            Gl.ReadPixels( x, y, width, height, format, type, ( IntBuffer )pixels );
        else if ( pixels is FloatBuffer )
            Gl.ReadPixels( x, y, width, height, format, type, ( FloatBuffer )pixels );
        else

            throw new GdxRuntimeException(
                $"Can't use {pixels.GetType().Name} with this method. Use ByteBuffer, ShortBuffer, IntBuffer or FloatBuffer instead." );
    }

    public void GLReleaseShaderCompiler()
    {
        // nothing to do here
    }

    public void GLRenderbufferStorage( int target, int internalformat, int width, int height )
    {
        EXTFramebufferObject.glRenderbufferStorageEXT( target, internalformat, width, height );
    }

    public void GLSampleCoverage( float value, bool invert )
    {
        Gl.SampleCoverage( value, invert );
    }

    public void GLScissor( int x, int y, int width, int height )
    {
        Gl.Scissor( x, y, width, height );
    }

    public void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length )
    {
        throw new UnsupportedOperationException( "unsupported, won't implement" );
    }

    public void GLShaderSource( int shader, String string )
    {
        Gl.ShaderSource( shader, string );
    }

    public void GLStencilFunc( int func, int ref, int Mask) {
        Gl.StencilFunc( func, ref, mask );
    }

    public void GLStencilFuncSeparate( int face, int func, int ref, int Mask) {
        Gl.StencilFuncSeparate( face, func, ref, mask );
    }

    public void GLStencilMask( int mask )
    {
        Gl.StencilMask( mask );
    }

    public void GLStencilMaskSeparate( int face, int mask )
    {
        Gl.StencilMaskSeparate( face, mask );
    }

    public void GLStencilOp( int fail, int zfail, int zpass )
    {
        Gl.StencilOp( fail, zfail, zpass );
    }

    public void GLStencilOpSeparate( int face, int fail, int zfail, int zpass )
    {
        Gl.StencilOpSeparate( face, fail, zfail, zpass );
    }

    public void GLTexImage2D( int target,
                              int level,
                              int internalformat,
                              int width,
                              int height,
                              int border,
                              int format,
                              int type,
                              Buffer pixels )
    {
        if ( pixels == null )
        {
            Gl.TexImage2D( target, level, internalformat, width, height, border, format, type, ( ByteBuffer )null );
        }
        else if ( pixels
                 is ByteBuffer )
            Gl.TexImage2D( target, level, internalformat, width, height, border, format, type, ( ByteBuffer )pixels );
        else if ( pixels is ShortBuffer )
            Gl.TexImage2D( target, level, internalformat, width, height, border, format, type, ( ShortBuffer )pixels );
        else if ( pixels is IntBuffer )
            Gl.TexImage2D( target, level, internalformat, width, height, border, format, type, ( IntBuffer )pixels );
        else if ( pixels is FloatBuffer )
            Gl.TexImage2D( target, level, internalformat, width, height, border, format, type, ( FloatBuffer )pixels );
        else if ( pixels is DoubleBuffer )
            Gl.TexImage2D( target, level, internalformat, width, height, border, format, type, ( DoubleBuffer )pixels );
        else

            throw new GdxRuntimeException(
                $"Can't use {pixels.GetType().Name} with this method. Use ByteBuffer, ShortBuffer, IntBuffer, FloatBuffer or DoubleBuffer instead." );
    }

    public void GLTexParameterf( int target, int pname, float param )
    {
        Gl.TexParameterf( target, pname, param );
    }

    public void GLTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
        Gl.TexParameterfv( target, pname, parameters );
    }

    public void GLTexParameteri( int target, int pname, int param )
    {
        Gl.TexParameteri( target, pname, param );
    }

    public void GLTexParameteriv( int target, int pname, IntBuffer parameters )
    {
        Gl.TexParameteriv( target, pname, parameters );
    }

    public void GLTexSubImage2D( int target,
                                 int level,
                                 int xoffset,
                                 int yoffset,
                                 int width,
                                 int height,
                                 int format,
                                 int type,
                                 Buffer pixels )
    {
        if ( pixels is ByteBuffer )
            Gl.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, ( ByteBuffer )pixels );
        else if ( pixels is ShortBuffer )
            Gl.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, ( ShortBuffer )pixels );
        else if ( pixels is IntBuffer )
            Gl.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, ( IntBuffer )pixels );
        else if ( pixels is FloatBuffer )
            Gl.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, ( FloatBuffer )pixels );
        else if ( pixels is DoubleBuffer )
            Gl.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, ( DoubleBuffer )pixels );
        else

            throw new GdxRuntimeException(
                $"Can't use {pixels.GetType().Name} with this method. Use ByteBuffer, ShortBuffer, IntBuffer, FloatBuffer or DoubleBuffer instead." );
    }

    public void GLUniform1F( int location, float x )
    {
        Gl.Uniform1f( location, x );
    }

    public void GLUniform1Fv( int location, int count, FloatBuffer v )
    {
        Gl.Uniform1fv( location, v );
    }

    public void GLUniform1Fv( int location, int count, float[] v, int offset )
    {
        Gl.Uniform1fv( location, toFloatBuffer( v, offset, count ) );
    }

    public void GLUniform1I( int location, int x )
    {
        Gl.Uniform1i( location, x );
    }

    public void GLUniform1Iv( int location, int count, IntBuffer v )
    {
        Gl.Uniform1iv( location, v );
    }

    @Override

    public void GLUniform1Iv( int location, int count, int[] v, int offset )
    {
        Gl.Uniform1iv( location, toIntBuffer( v, offset, count ) );
    }

    public void GLUniform2F( int location, float x, float y )
    {
        Gl.Uniform2f( location, x, y );
    }

    public void GLUniform2Fv( int location, int count, FloatBuffer v )
    {
        Gl.Uniform2fv( location, v );
    }

    public void GLUniform2Fv( int location, int count, float[] v, int offset )
    {
        Gl.Uniform2fv( location, toFloatBuffer( v, offset, count << 1 ) );
    }

    public void GLUniform2I( int location, int x, int y )
    {
        Gl.Uniform2i( location, x, y );
    }

    public void GLUniform2Iv( int location, int count, IntBuffer v )
    {
        Gl.Uniform2iv( location, v );
    }

    public void GLUniform2Iv( int location, int count, int[] v, int offset )
    {
        Gl.Uniform2iv( location, toIntBuffer( v, offset, count << 1 ) );
    }

    public void GLUniform3F( int location, float x, float y, float z )
    {
        Gl.Uniform3f( location, x, y, z );
    }

//    public void GLUniform3Fv( int location, int count, FloatBuffer v )
//    {
//        Gl.Uniform3fv( location, v );
//    }

//    public void GLUniform3Fv( int location, int count, float[] v, int offset )
//    {
//        Gl.Uniform3fv( location, toFloatBuffer( v, offset, count * 3 ) );
//    }

    public void GLUniform3I<T>( int location, int count, int value ) where T : struct
    {
        Gl.Uniform3i( location, count, value );
    }

//    public void GLUniform3Iv( int location, int count, IntBuffer v )
//    {
//        Gl.Uniform3iv( location, v );
//    }

//    public void GLUniform3Iv( int location, int count, int[] v, int offset )
//    {
//        Gl.Uniform3iv( location, ToIntBuffer( v, offset, count * 3 ) );
//    }

    /// <summary>
    /// Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">
    /// Specifies the location of the uniform variable to be modified.
    /// </param>
    /// <param name="count">
    /// For the vector (Gl.Uniform*v) commands, specifies the number of elements that are to be
    /// modified. This should be 1 if the targeted uniform variable is not an array, and 1 or
    /// more if it is an array.
    /// </param>
    /// <param name="value">
    /// For the vector and matrix commands, specifies a pointer to an array of <paramref name="count" />
    /// values that will be used to update the specified uniform variable.
    /// </param>
    public void GLUniform4F<T>( int location, int count, T value ) where T : struct
    {
        Gl.Uniform4f( location, count, value );
    }

//    public void GLUniform4Fv( int location, int count, FloatBuffer v )
//    {
//        Gl.Uniform4fv( location, v );
//    }

//    public void GLUniform4Fv( int location, int count, float[] v, int offset )
//    {
//        Gl.Uniform4fv( location, ToFloatBuffer( v, offset, count << 2 ) );
//    }

//    public void GLUniform4I( int location, int x, int y, int z, int w )
//    {
//        Gl.Uniform4i( location, x, y, z, w );
//    }

//    public void GLUniform4Iv( int location, int count, IntBuffer v )
//    {
//        Gl.Uniform4iv( location, v );
//    }

//    public void GLUniform4Iv( int location, int count, int[] v, int offset )
//    {
//        Gl.Uniform4iv( location, toIntBuffer( v, offset, count << 2 ) );
//    }

//    public void GLUniformMatrix2Fv( int location, int count, bool transpose, FloatBuffer value )
//    {
//        Gl.UniformMatrix2fv( location, transpose, value );
//    }

//    public void GLUniformMatrix2Fv( int location, int count, bool transpose, float[] value, int offset )
//    {
//        Gl.UniformMatrix2fv( location, transpose, ToFloatBuffer( value, offset, count << 2 ) );
//    }

//    public void GLUniformMatrix3Fv( int location, int count, bool transpose, FloatBuffer value )
//    {
//        Gl.UniformMatrix3fv( location, transpose, value );
//    }

//    public void GLUniformMatrix3Fv( int location, int count, bool transpose, float[] value, int offset )
//    {
//        Gl.UniformMatrix3fv( location, transpose, ToFloatBuffer( value, offset, count * 9 ) );
//    }

//    public void GLUniformMatrix4Fv( int location, int count, bool transpose, FloatBuffer value )
//    {
//        Gl.UniformMatrix4fv( location, transpose, value );
//    }

//    public void GLUniformMatrix4Fv( int location, int count, bool transpose, float[] value, int offset )
//    {
//        Gl.UniformMatrix4fv( location, transpose, ToFloatBuffer( value, offset, count << 4 ) );
//    }

    public void GLUseProgram( uint program )
    {
        Gl.UseProgram( program );
    }

    public void GLValidateProgram( uint program )
    {
        Gl.ValidateProgram( program );
    }

    public void GLVertexAttrib1F( uint indx, float x )
    {
        Gl.VertexAttrib1f( indx, x );
    }

    public void GLVertexAttrib1Fv( uint indx, FloatBuffer values )
    {
        Gl.VertexAttrib1f( indx, values.Get() );
    }

//    public void GLVertexAttrib2F( int indx, float x, float y )
//    {
//        //TODO:
//        throw new NotImplementedException();
//    }

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="indx">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="values">
    /// For the vector commands (Gl.VertexAttrib*v), specifies a pointer to an array
    /// of values to be used for the generic vertex attribute.
    /// </param>
    public void GLVertexAttrib2Fv( uint indx, FloatBuffer values )
    {
        Gl.VertexAttrib2f( indx, values.Get() );
    }

//    public void GLVertexAttrib3F( int indx, float x, float y, float z )
//    {
//        //TODO:
//        throw new NotImplementedException();
//    }

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="indx">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="values">
    /// For the vector commands (Gl.VertexAttrib*v), specifies a pointer to an array
    /// of values to be used for the generic vertex attribute.
    /// </param>
    public void GLVertexAttrib3Fv( uint indx, FloatBuffer values )
    {
        Gl.VertexAttrib3f( indx, values.Get() );
    }

//    public void GLVertexAttrib4F( int indx, float x, float y, float z, float w )
//    {
//        //TODO:
//        throw new NotImplementedException();
//    }

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="indx">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="values">
    /// For the vector commands (Gl.VertexAttrib*v), specifies a pointer to an array
    /// of values to be used for the generic vertex attribute.
    /// </param>
    public void GLVertexAttrib4Fv( uint indx, FloatBuffer values )
    {
        Gl.VertexAttrib4f( indx, values.Get() );
    }

    /// <summary>
    /// define an array of generic vertex attribute data.
    /// </summary>
    /// <param name="indx">
    /// Specifies the index of the generic vertex attribute to be modified.
    /// </param>
    /// <param name="size">
    /// Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4.
    /// Additionally, the symbolic constant Gl.BGRA is accepted by Gl.VertexAttribPointer.
    /// The initial value is 4.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of each component in the array. The symbolic constants Gl.BYTE,
    /// Gl.UNSIGNED_BYTE, Gl.SHORT, Gl.UNSIGNED_SHORT, Gl.INT, and Gl.UNSIGNED_INT are accepted
    /// by Gl.VertexAttribPointer and Gl.VertexAttribIPointer. Additionally Gl.HALF_FLOAT, Gl.FLOAT,
    /// Gl.DOUBLE, Gl.FIXED, Gl.INT_2_10_10_10_REV, Gl.UNSIGNED_INT_2_10_10_10_REV and
    /// Gl.UNSIGNED_INT_10F_11F_11F_REV are accepted by Gl.VertexAttribPointer. Gl.DOUBLE is also
    /// accepted by Gl.VertexAttribLPointer and is the only token accepted by the type parameter
    /// for that function. The initial value is Gl.FLOAT.
    /// </param>
    /// <param name="normalized">
    /// For Gl.VertexAttribPointer, specifies whether fixed-point data values should be normalized
    /// (Gl.TRUE) or converted directly as fixed-point values (Gl.FALSE) when they are accessed.
    /// </param>
    /// <param name="stride">
    /// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the
    /// generic vertex attributes are understood to be tightly packed in the array.
    /// The initial value is 0.
    /// </param>
    /// <param name="pointer">
    /// Specifies a offset of the first component of the first generic vertex attribute in the array
    /// in the data store of the buffer currently bound to the Gl.ARRAY_BUFFER target.
    /// The initial value is 0.
    /// </param>
    public void GLVertexAttribPointer( uint indx,
                                       int size,
                                       VertexAttribType type,
                                       bool normalized,
                                       int stride,
                                       Buffer pointer )
    {
        if ( pointer is ByteBuffer byteBuffer )
        {
            if ( type == VertexAttribType.Byte )
            {
                Gl.VertexAttribPointer( indx, size, type, normalized, stride, byteBuffer );
            }
            else if ( type == VertexAttribType.UnsignedByte )
            {
                Gl.VertexAttribPointer( indx, size, type, normalized, stride, byteBuffer );
            }
            else if ( type == VertexAttribType.Short )
            {
                Gl.VertexAttribPointer( indx, size, type, normalized, stride, byteBuffer.AsShortBuffer() );
            }
            else if ( type == VertexAttribType.UnsignedShort )
            {
                Gl.VertexAttribPointer( indx, size, type, normalized, stride, byteBuffer.AsShortBuffer() );
            }
            else if ( type == VertexAttribType.Float )
            {
                Gl.VertexAttribPointer( indx, size, type, normalized, stride, byteBuffer.AsFloatBuffer() );
            }
            else
            {
                throw new GdxRuntimeException( $"Can't use {byteBuffer.GetType().Name} with type {type} with this "
                                             + $"method. Use ByteBuffer and one of GL_BYTE, GL_UNSIGNED_BYTE, "
                                             + $"GL_SHORT, GL_UNSIGNED_SHORT or GL_FLOAT for type." );
            }
        }
        else if ( pointer is FloatBuffer fBuffer )
        {
            if ( type == VertexAttribType.Float )
            {
                Gl.VertexAttribPointer( indx, size, type, normalized, stride, fBuffer );
            }
            else
            {
                throw new GdxRuntimeException( $"Can't use {fBuffer.GetType().Name} with type {type} with this method." );
            }
        }
        else
        {
            throw new GdxRuntimeException( $"Can't use {pointer.GetType().Name} with this method. Use ByteBuffer instead." );
        }
    }

    /// <summary>
    /// Set the viewport.
    /// </summary>
    /// <param name="x">
    /// Specify the lower left corner of the viewport rectangle, in pixels. The initial value is (0,0).
    /// </param>
    /// <param name="y">
    /// Specify the lower left corner of the viewport rectangle, in pixels. The initial value is (0,0).
    /// </param>
    /// <param name="width">
    /// Specify the width and height of the viewport. When a GL context is first attached to a window,
    /// width and height are set to the dimensions of that window.
    /// </param>
    /// <param name="height">
    /// Specify the width and height of the viewport. When a GL context is first attached to a window,
    /// width and height are set to the dimensions of that window.
    /// </param>
    public void GLViewport( int x, int y, int width, int height )
    {
        Gl.Viewport( x, y, width, height );
    }

    /// <summary>
    /// render primitives from array data.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants Gl.POINTS, Gl.LINE_STRIP, Gl.LINE_LOOP,
    /// Gl.LINES, Gl.LINE_STRIP_ADJACENCY, Gl.LINES_ADJACENCY, Gl.TRIANGLE_STRIP, Gl.TRIANGLE_FAN, Gl.TRIANGLES,
    /// Gl.TRIANGLE_STRIP_ADJACENCY, Gl.TRIANGLES_ADJACENCY and Gl.PATCHES are accepted.
    /// </param>
    /// <param name="count"> Specifies the number of elements to be rendered. </param>
    /// <param name="type">
    /// Specifies the type of the values in indices. Must be one of Gl.UNSIGNED_BYTE, Gl.UNSIGNED_SHORT,
    /// or Gl.UNSIGNED_INT.
    /// </param>
    /// <param name="indices"> Specifies a pointer to the location where the indices are stored. </param>
    public void GLDrawElements( PrimitiveType mode, int count, DrawElementsType type, int indices )
    {
        Gl.DrawElements( mode, count, type, indices );
    }

    /// <summary>
    /// define an array of generic vertex attribute data.
    /// </summary>
    /// <param name="indx">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="size">
    /// Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4. Additionally,
    /// the symbolic constant Gl.BGRA is accepted by Gl.VertexAttribPointer. The initial value is 4.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of each component in the array. The symbolic constants Gl.BYTE, Gl.UNSIGNED_BYTE,
    /// Gl.SHORT, Gl.UNSIGNED_SHORT, Gl.INT, and Gl.UNSIGNED_INT are accepted by Gl.VertexAttribPointer and
    /// Gl.VertexAttribIPointer. Additionally Gl.HALF_FLOAT, Gl.FLOAT, Gl.DOUBLE, Gl.FIXED, Gl.INT_2_10_10_10_REV,
    /// Gl.UNSIGNED_INT_2_10_10_10_REV and Gl.UNSIGNED_INT_10F_11F_11F_REV are accepted by Gl.VertexAttribPointer.
    /// Gl.DOUBLE is also accepted by Gl.VertexAttribLPointer and is the only token accepted by the type parameter
    /// for that function. The initial value is Gl.FLOAT.
    /// </param>
    /// <param name="normalized">
    /// For Gl.VertexAttribPointer, specifies whether fixed-point data values should be normalized (Gl.TRUE) or
    /// converted directly as fixed-point values (Gl.FALSE) when they are accessed.
    /// </param>
    /// <param name="stride">
    /// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the generic vertex
    /// attributes are understood to be tightly packed in the array. The initial value is 0.
    /// </param>
    /// <param name="ptr">
    /// Specifies a offset of the first component of the first generic vertex attribute in the array in the data
    /// store of the buffer currently bound to the Gl.ARRAY_BUFFER target. The initial value is 0.
    /// </param>
    public void GLVertexAttribPointer( uint indx, int size, VertexAttribType type, bool normalized, int stride, int ptr )
    {
        Gl.VertexAttribPointer( indx, size, type, normalized, stride, ptr );
    }
}
