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

using System.Text;

using LibGDXSharp.Utils.Buffers;

using Buffer = LibGDXSharp.Utils.Buffers.Buffer;
using ErrorCode = OpenGL.ErrorCode;

namespace LibGDXSharp.Backends.Desktop.Graphics;

[PublicAPI]
public class DesktopGL20 : IGL20
{
    private ByteBuffer?  _buffer      = null;
    private FloatBuffer? _floatBuffer = null;
    private IntBuffer?   _intBuffer   = null;

    /// <inheritdoc cref="Gl.BlendColor(float,float,float,float)" />
    public void GLBlendColor( float red, float green, float blue, float alpha ) => Gl.BlendColor( red, green, blue, alpha );

    /// <inheritdoc cref="Gl.ClearColor(float,float,float,float)" />
    public void GLClearColor( float red, float green, float blue, float alpha ) => Gl.ClearColor( red, green, blue, alpha );

    /// <inheritdoc cref="Gl.ClearDepth(float)" />
    public void GLClearDepthf( float depth ) => Gl.ClearDepth( depth );

    /// <inheritdoc cref="Gl.ClearStencil(int)" />
    public void GLClearStencil( int s ) => Gl.ClearStencil( s );

    /// <inheritdoc cref="Gl.ColorMask(bool,bool,bool,bool)" />
    public void GLColorMask( bool red, bool green, bool blue, bool alpha ) => Gl.ColorMask( red, green, blue, alpha );

    public void GLDepthMask( bool flag ) => Gl.DepthMask( flag );

    public void GLDepthRangef( float zNear, float zFar ) => Gl.DepthRange( zNear, zFar );

    /// <inheritdoc cref="Gl.Finish()" />
    public void GLFinish() => Gl.Finish();

    /// <inheritdoc cref="Gl.Flush()" />
    public void GLFlush() => Gl.Flush();

    /// <inheritdoc cref="Gl.LineWidth(float)" />
    public void GLLineWidth( float width ) => Gl.LineWidth( width );

    /// <inheritdoc cref="Gl.PolygonOffset(float,float)" />
    public void GLPolygonOffset( float factor, float units ) => Gl.PolygonOffset( factor, units );

    //    public void GLReleaseShaderCompiler()
//    {
//        // nothing to do here
//    }

//    public void GLRenderbufferStorage( int target, int internalformat, int width, int height )
//    {
//        EXTFramebufferObject.glRenderbufferStorageEXT( target, internalformat, width, height );
//    }

    /// <inheritdoc cref="Gl.SampleCoverage(float,bool)" />
    public void GLSampleCoverage( float value, bool invert ) => Gl.SampleCoverage( value, invert );

    /// <inheritdoc cref="Gl.Scissor(int,int,int,int)" />
    public void GLScissor( int x, int y, int width, int height ) => Gl.Scissor( x, y, width, height );

    /// <inheritdoc cref="Gl.Viewport(int,int,int,int)" />
    public void GLViewport( int x, int y, int width, int height ) => Gl.Viewport( x, y, width, height );

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

    /// <inheritdoc cref="Gl.ActiveTexture(TextureUnit)" />
    public void GLActiveTexture( TextureUnit texture ) => Gl.ActiveTexture( texture );

    /// <inheritdoc cref="Gl.AttachShader(uint,uint)" />
    public void GLAttachShader( uint program, uint shader ) => Gl.AttachShader( program, shader );

    /// <inheritdoc cref="Gl.BindTexture(TextureTarget,uint)" />
    public void GLBindTexture( TextureTarget target, uint texture ) => Gl.BindTexture( target, texture );

    /// <inheritdoc cref="Gl.BindAttribLocation(uint,uint,string)" />
    public void GLBindAttribLocation( uint program, uint index, string name ) => Gl.BindAttribLocation( program, index, name );

    /// <inheritdoc cref="Gl.BindBuffer(BufferTarget,uint)" />
    public void GLBindBuffer( BufferTarget target, uint buffer ) => Gl.BindBuffer( target, buffer );

    /// <inheritdoc cref="Gl.BindFramebufferEXT(FramebufferTarget,uint)" />
    public void GLBindFramebuffer( FramebufferTarget target, uint framebuffer ) => Gl.BindFramebufferEXT( target, framebuffer );

    /// <inheritdoc cref="Gl.BindRenderbufferEXT(RenderbufferTarget,uint)" />
    public void GLBindRenderbuffer( RenderbufferTarget target, uint renderbuffer ) => Gl.BindRenderbufferEXT( target, renderbuffer );

    /// <inheritdoc cref="Gl.BlendEquation(BlendEquationMode)" />
    public void GLBlendEquation( BlendEquationMode mode ) => Gl.BlendEquation( mode );

    /// <inheritdoc cref="Gl.BlendFuncSeparate(BlendingFactor,BlendingFactor,BlendingFactor,BlendingFactor)" />
    public void GLBlendEquationSeparate( BlendEquationMode modeRGB, BlendEquationMode modeAlpha ) => Gl.BlendEquationSeparate( modeRGB, modeAlpha );

    /// <inheritdoc cref="Gl.BlendFunc(BlendingFactor,BlendingFactor)" />
    public void GLBlendFunc( BlendingFactor sfactor, BlendingFactor dfactor ) => Gl.BlendFunc( sfactor, dfactor );

    /// <inheritdoc cref="Gl.BlendFuncSeparate(BlendingFactor,BlendingFactor,BlendingFactor,BlendingFactor)" />
    public void GLBlendFuncSeparate( BlendingFactor srcRGB,
                                     BlendingFactor dstRGB,
                                     BlendingFactor srcAlpha,
                                     BlendingFactor dstAlpha ) => Gl.BlendFuncSeparate( srcRGB, dstRGB, srcAlpha, dstAlpha );

    public void GLBufferData( BufferTarget target,
                              uint size,
                              Buffer? data,
                              OpenGL.BufferUsage usage )
    {
        switch ( data )
        {
            case null:
                Gl.BufferData( target, size, data, usage );

                break;

            case ByteBuffer byteBuffer:
                Gl.BufferData( target, size, byteBuffer, usage );

                break;

            case IntBuffer intBuffer:
                Gl.BufferData( target, size, intBuffer, usage );

                break;

            case FloatBuffer floatBuffer:
                Gl.BufferData( target, size, floatBuffer, usage );

                break;

            case DoubleBuffer doubleBuffer:
                Gl.BufferData( target, size, doubleBuffer, usage );

                break;

            case ShortBuffer shortBuffer:
                Gl.BufferData( target, size, shortBuffer, usage );

                break;
        }
    }

    public void GLBufferSubData( BufferTarget target, IntPtr offset, uint size, Buffer data )
    {
        ArgumentNullException.ThrowIfNull( data );

        switch ( data )
        {
            case ByteBuffer byteBuffer:
                Gl.BufferSubData( target, offset, size, byteBuffer );

                break;

            case IntBuffer intBuffer:
                Gl.BufferSubData( target, offset, size, intBuffer );

                break;

            case FloatBuffer floatBuffer:
                Gl.BufferSubData( target, offset, size, floatBuffer );

                break;

            case DoubleBuffer doubleBuffer:
                Gl.BufferSubData( target, offset, size, doubleBuffer );

                break;

            case ShortBuffer shortBuffer:
                Gl.BufferSubData( target, offset, size, shortBuffer );

                break;
        }
    }

//    public int GLCheckFramebufferStatus( int target )
//    {
//        return EXTFramebufferObject.glCheckFramebufferStatusEXT( target );
//    }

    /// <inheritdoc cref="Gl.Clear(ClearBufferMask)" />
    public void GLClear( ClearBufferMask mask ) => Gl.Clear( mask );

    /// <inheritdoc cref="Gl.CompileShader(uint)" />
    public void GLCompileShader( uint shader ) => Gl.CompileShader( shader );

    public void GLCompressedTexImage2D( TextureTarget target,
                                        int level,
                                        InternalFormat internalformat,
                                        int width,
                                        int height,
                                        int border,
                                        int imageSize,
                                        Buffer data )
    {
        if ( data is ByteBuffer buffer )
        {
            Gl.CompressedTexImage2D( target, level, internalformat, width, height, border, imageSize, buffer );
        }
        else
        {
            throw new GdxRuntimeException( $"Can't use {data.GetType().Name} with this method. Use ByteBuffer instead." );
        }
    }

//    public void GLCompressedTexSubImage2D( int target,
//                                           int level,
//                                           int xoffset,
//                                           int yoffset,
//                                           int width,
//                                           int height,
//                                           int format,
//                                           int imageSize,
//                                           Buffer data )
//    {
//        throw new GdxRuntimeException( "not implemented" );
//    }

    /// <inheritdoc cref="Gl.CopyTexImage2D(TextureTarget,int,InternalFormat,int,int,int,int,int)" />
    public void GLCopyTexImage2D( TextureTarget target,
                                  int level,
                                  InternalFormat internalformat,
                                  int x,
                                  int y,
                                  int width,
                                  int height,
                                  int border ) => Gl.CopyTexImage2D( target, level, internalformat, x, y, width, height, border );

    /// <inheritdoc cref="Gl.CopyTexSubImage2D(TextureTarget,int,int,int,int,int,int,int)" />
    public void GLCopyTexSubImage2D( TextureTarget target, int level, int xoffset, int yoffset, int x, int y, int width, int height )
        => Gl.CopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );

    /// <inheritdoc cref="Gl.CreateProgram()" />
    public uint GLCreateProgram() => Gl.CreateProgram();

    /// <inheritdoc cref="Gl.CreateShader(ShaderType)" />
    public uint GLCreateShader( ShaderType type ) => Gl.CreateShader( type );

    /// <inheritdoc cref="Gl.CullFace(CullFaceMode)" />
    public void GLCullFace( CullFaceMode mode ) => Gl.CullFace( mode );

    /// <inheritdoc cref="Gl.DeleteBuffers(uint[])" />
    public void GLDeleteBuffers( params uint[] buffers ) => Gl.DeleteBuffers( buffers );

    //    public void GLDeleteFramebuffers( int n, IntBuffer framebuffers )
//    {
//        EXTFramebufferObject.glDeleteFramebuffersEXT( framebuffers );
//    }

//    public void GLDeleteFramebuffer( int framebuffer )
//    {
//        EXTFramebufferObject.glDeleteFramebuffersEXT( framebuffer );
//    }

    /// <inheritdoc cref="Gl.DeleteProgram(uint)" />
    public void GLDeleteProgram( uint program ) => Gl.DeleteProgram( program );

    //    public void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers )
//    {
//        EXTFramebufferObject.glDeleteRenderbuffersEXT( renderbuffers );
//    }

//    public void GLDeleteRenderbuffer( int renderbuffer )
//    {
//        EXTFramebufferObject.glDeleteRenderbuffersEXT( renderbuffer );
//    }

    /// <inheritdoc cref="Gl.DeleteShader(uint)" />
    public void GLDeleteShader( uint shader ) => Gl.DeleteShader( shader );

    /// <inheritdoc cref="Gl.DeleteTextures(uint[])" />
    public void GLDeleteTextures( params uint[] textures ) => Gl.DeleteTextures( textures );

    /// <inheritdoc cref="Gl.DepthFunc(DepthFunction)" />
    public void GLDepthFunc( DepthFunction func ) => Gl.DepthFunc( func );

    public void GLDetachShader( uint program, uint shader ) => Gl.DetachShader( program, shader );

    public void GLDisable( EnableCap cap ) => Gl.Disable( cap );

    public void GLDisableVertexAttribArray( uint index ) => Gl.DisableVertexAttribArray( index );

    public void GLDrawArrays( PrimitiveType mode, int first, int count ) => Gl.DrawArrays( mode, first, count );

    public void GLDrawElements( PrimitiveType mode, int count, DrawElementsType type, Buffer indices )
    {
        if ( indices is ShortBuffer buffer && ( type == DrawElementsType.UnsignedShort ) )
        {
            Gl.DrawElements( mode, count, type, buffer );
        }
        else if ( indices is ByteBuffer byteBuf1 && ( type == DrawElementsType.UnsignedShort ) )
        {
            Gl.DrawElements( mode, count, type, byteBuf1.AsShortBuffer() );
        }
        else if ( indices is ByteBuffer byteBuf2 && ( type == DrawElementsType.UnsignedByte ) )
        {
            Gl.DrawElements( mode, count, type, byteBuf2 );
        }
        else
        {
            throw new GdxRuntimeException( $"Can't use {indices.GetType().Name} with this "
                                         + $"method. Use ShortBuffer or ByteBuffer instead." );
        }
    }

    /// <inheritdoc cref="Gl.Enable(EnableCap)" />
    public void GLEnable( EnableCap cap ) => Gl.Enable( cap );

    /// <inheritdoc cref="Gl.EnableVertexAttribArray(uint)" />
    public void GLEnableVertexAttribArray( uint index ) => Gl.EnableVertexAttribArray( index );

    //    public void GLFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, int renderbuffer )
//    {
//        EXTFramebufferObject.glFramebufferRenderbufferEXT( target, attachment, renderbuffertarget, renderbuffer );
//    }

//    public void GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level )
//    {
//        EXTFramebufferObject.glFramebufferTexture2DEXT( target, attachment, textarget, texture, level );
//    }

    /// <inheritdoc cref="Gl.FrontFace(FrontFaceDirection)" />
    public void GLFrontFace( FrontFaceDirection mode ) => Gl.FrontFace( mode );

    /// <inheritdoc cref="Gl.GenBuffers(uint[])" />
    public void GLGenBuffers( uint[] buffers ) => Gl.GenBuffers( buffers );

    /// <inheritdoc cref="Gl.GenBuffer()" />
    public uint GLGenBuffer() => Gl.GenBuffer();

    //    public void GLGenFramebuffers( int n, IntBuffer framebuffers )
//    {
//        EXTFramebufferObject.glGenFramebuffersEXT( framebuffers );
//    }

//    public int GLGenFramebuffer()
//    {
//        return EXTFramebufferObject.glGenFramebuffersEXT();
//    }

//    public void GLGenRenderbuffers( int n, IntBuffer renderbuffers )
//    {
//        EXTFramebufferObject.glGenRenderbuffersEXT( renderbuffers );
//    }

//    public int GLGenRenderbuffer()
//    {
//        return EXTFramebufferObject.glGenRenderbuffersEXT();
//    }

    /// <inheritdoc cref="Gl.GenTextures(uint[])" />
    public void GLGenTextures( uint[] textures ) => Gl.GenTextures( textures );

    /// <inheritdoc cref="Gl.GenTexture()" />
    public uint GLGenTexture() => Gl.GenTexture();

    //    public void GLGenerateMipmap( int target )
//    {
//        EXTFramebufferObject.glGenerateMipmapEXT( target );
//    }

    /// <inheritdoc cref="Gl.GetActiveAttrib(uint,uint,int,out int,out int,out int,StringBuilder)" />
    public void GLGetActiveAttrib( uint program,
                                   uint index,
                                   int bufSize,
                                   out int length,
                                   out int size,
                                   out int type,
                                   StringBuilder name ) => Gl.GetActiveAttrib( program, index, bufSize, out length, out size, out type, name );

    /// <inheritdoc cref="Gl.GetActiveUniform(uint,uint,int,out int, out int,out int,StringBuilder)" />
    public void GLGetActiveUniform( uint program,
                                    uint index,
                                    int bufSize,
                                    out int length,
                                    out int size,
                                    out int type,
                                    StringBuilder name ) => Gl.GetActiveUniform( program, index, bufSize, out length, out size, out type, name );

    /// <inheritdoc cref="Gl.GetAttachedShaders(uint,out int,uint[])" />
    public void GLGetAttachedShaders( uint program, out int buffer, uint[] shaders ) => Gl.GetAttachedShaders( program, out buffer, shaders );

    /// <inheritdoc cref="Gl.GetAttribLocation(uint,string)" />
    public int GLGetAttribLocation( uint program, string name ) => Gl.GetAttribLocation( program, name );

    //    public void GLGetBooleanv( int pname, Buffer parameters )
//    {
//        Gl.GetBooleanv( pname, ( ByteBuffer )parameters );
//    }

//    public void GLGetBufferParameteriv( int target, int pname, IntBuffer parameters )
//    {
//        Gl.GetBufferParameteriv( target, pname, parameters );
//    }

    /// <inheritdoc cref="Gl.GetError()" />
    public ErrorCode GLGetError() => Gl.GetError();

    public void GLGetFloatv<T>( GetPName pname, out T parameters ) where T : struct => Gl.GetFloat( pname, out parameters );

    //    public void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters )
//    {
//        EXTFramebufferObject.glGetFramebufferAttachmentParameterivEXT( target, attachment, pname, parameters );
//    }

//    public void GLGetIntegerv( int pname, IntBuffer parameters )
//    {
//        Gl.GetIntegerv( pname, parameters );
//    }

//    public string GLGetProgramInfoLog( uint program )
//    {
//        ByteBuffer buffer = ByteBuffer.AllocateDirect( 1024 * 10 );

//        buffer.Order( ByteOrder.NativeOrder );

//        ByteBuffer tmp = ByteBuffer.AllocateDirect( 4 );
//        tmp.Order( ByteOrder.NativeOrder );

//        IntBuffer intBuffer = tmp.AsIntBuffer();

//        Gl.GetProgramInfoLog( program, intBuffer, buffer );

//        var numBytes = intBuffer.Get( 0 );
//        var bytes    = new byte[ numBytes ];

//        buffer.Get( bytes );

//        return new string( bytes );
//    }

//    public void GLGetProgramiv( int program, int pname, IntBuffer parameters )
//    {
//        Gl.GetProgramiv( program, pname, parameters );
//    }

//    public void GLGetRenderbufferParameteriv( int target, int pname, IntBuffer parameters )
//    {
//        EXTFramebufferObject.glGetRenderbufferParameterivEXT( target, pname, parameters );
//    }

//    public string GLGetShaderInfoLog( int shader )
//    {
//        ByteBuffer buffer = ByteBuffer.allocateDirect( 1024 * 10 );
//        buffer.order( ByteOrder.nativeOrder() );
//        ByteBuffer tmp = ByteBuffer.allocateDirect( 4 );
//        tmp.order( ByteOrder.nativeOrder() );
//        IntBuffer intBuffer = tmp.asIntBuffer();

//        Gl.GetShaderInfoLog( shader, intBuffer, buffer );
//        int    numBytes = intBuffer.get( 0 );
//        byte[] bytes    = new byte[ numBytes ];
//        buffer.get( bytes );

//        return new string( bytes );
//    }

//    public void GLGetShaderPrecisionFormat( int shadertype, int precisiontype, IntBuffer range, IntBuffer precision )
//    {
//        throw new UnsupportedOperationException( "unsupported, won't implement" );
//    }

//    public void GLGetShaderiv( int shader, int pname, IntBuffer parameters )
//    {
//        Gl.GetShaderiv( shader, pname, parameters );
//    }

    /// <inheritdoc cref="Gl.GetString(StringName)" />
    public string GLGetString( StringName name ) => Gl.GetString( name );

    //    public void GLGetTexParameterfv( int target, int pname, FloatBuffer parameters )
//    {
//        Gl.GetTexParameterfv( target, pname, parameters );
//    }

//    public void GLGetTexParameteriv( int target, int pname, IntBuffer parameters )
//    {
//        Gl.GetTexParameteriv( target, pname, parameters );
//    }

    /// <inheritdoc cref="Gl.GetUniformLocation(uint,string)" />
    public int GLGetUniformLocation( uint program, string name ) => Gl.GetUniformLocation( program, name );

    //    public void GLGetUniformfv( int program, int location, FloatBuffer parameters )
//    {
//        Gl.GetUniformfv( program, location, parameters );
//    }

//    public void GLGetUniformiv( int program, int location, IntBuffer parameters )
//    {
//        Gl.GetUniformiv( program, location, parameters );
//    }

//    public void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer )
//    {
//        throw new UnsupportedOperationException( "unsupported, won't implement" );
//    }

//    public void GLGetVertexAttribfv( int index, int pname, FloatBuffer parameters )
//    {
//        Gl.GetVertexAttribfv( index, pname, parameters );
//    }

//    public void GLGetVertexAttribiv( int index, int pname, IntBuffer parameters )
//    {
//        Gl.GetVertexAttribiv( index, pname, parameters );
//    }

    /// <inheritdoc cref="Gl.Hint(HintTarget,HintMode)" />
    public void GLHint( HintTarget target, HintMode mode ) => Gl.Hint( target, mode );

    /// <inheritdoc cref="Gl.IsBuffer(uint)" />
    public bool GLIsBuffer( uint buffer ) => Gl.IsBuffer( buffer );

    /// <inheritdoc cref="Gl.IsEnabled(EnableCap)" />
    public bool GLIsEnabled( EnableCap cap ) => Gl.IsEnabled( cap );

    //    public bool GLIsFramebuffer( int framebuffer )
//    {
//        return EXTFramebufferObject.glIsFramebufferEXT( framebuffer );
//    }

    /// <inheritdoc cref="Gl.IsProgram(uint)" />
    public bool GLIsProgram( uint program ) => Gl.IsProgram( program );

    //    public bool GLIsRenderbuffer( int renderbuffer )
//    {
//        return EXTFramebufferObject.glIsRenderbufferEXT( renderbuffer );
//    }

    /// <inheritdoc cref="Gl.IsShader(uint)" />
    public bool GLIsShader( uint shader ) => Gl.IsShader( shader );

    /// <inheritdoc cref="Gl.IsTexture(uint)" />
    public bool GLIsTexture( uint texture ) => Gl.IsTexture( texture );

    /// <inheritdoc cref="Gl.LinkProgram(uint)" />
    public void GLLinkProgram( uint program ) => Gl.LinkProgram( program );

    public void GLPixelStorei( PixelStoreParameter pname, int param ) => Gl.PixelStore( pname, param );

    /// <inheritdoc cref="Gl.ReadPixels(int,int,int,int,PixelFormat,PixelType,IntPtr)" />
    public void GLReadPixels( int x, int y, int width, int height, PixelFormat format, PixelType type, IntPtr pixels )
        => Gl.ReadPixels( x, y, width, height, format, type, pixels );

    //    public void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length )
//    {
//        throw new UnsupportedOperationException( "unsupported, won't implement" );
//    }

    /// <inheritdoc cref="Gl.ShaderSource(uint,string[])" />
    public void GLShaderSource( uint shader, string[] str ) => Gl.ShaderSource( shader, str );

    /// <inheritdoc cref="Gl.StencilFunc(StencilFunction,int,uint)" />
    public void GLStencilFunc( StencilFunction func, int reference, uint mask ) => Gl.StencilFunc( func, reference, mask );

    /// <inheritdoc cref="Gl.StencilFuncSeparate(StencilFaceDirection,StencilFunction,int,uint)" />
    public void GLStencilFuncSeparate( StencilFaceDirection face, StencilFunction func, int reference, uint mask )
        => Gl.StencilFuncSeparate( face, func, reference, mask );

    /// <inheritdoc cref="Gl.StencilMask(uint)" />
    public void GLStencilMask( uint mask ) => Gl.StencilMask( mask );

    /// <inheritdoc cref="Gl.StencilMaskSeparate(StencilFaceDirection,uint)" />
    public void GLStencilMaskSeparate( StencilFaceDirection face, uint mask ) => Gl.StencilMaskSeparate( face, mask );

    /// <inheritdoc cref="Gl.StencilOp(StencilOp,StencilOp,StencilOp)" />
    public void GLStencilOp( StencilOp fail, StencilOp zfail, StencilOp zpass ) => Gl.StencilOp( fail, zfail, zpass );

    /// <inheritdoc cref="Gl.StencilOpSeparate(StencilFaceDirection,StencilOp,StencilOp,StencilOp)" />
    public void GLStencilOpSeparate( StencilFaceDirection face, StencilOp fail, StencilOp zfail, StencilOp zpass )
        => Gl.StencilOpSeparate( face, fail, zfail, zpass );

    public void GLTexImage2D( TextureTarget target,
                              int level,
                              InternalFormat internalformat,
                              int width,
                              int height,
                              int border,
                              PixelFormat format,
                              PixelType type,
                              IntPtr pixels ) => Gl.TexImage2D( target, level, internalformat, width, height, border, format, type, pixels );

    /// <inheritdoc cref="Gl.TexParameterf{T}(TextureTarget,TextureParameterName,T)" />
    public void GLTexParameterf<T>( TextureTarget target, TextureParameterName pname, T param ) where T : struct => Gl.TexParameterf( target, pname, param );

    //    public void GLTexParameterfv( int target, int pname, FloatBuffer parameters )
//    {
//        Gl.TexParameterfv( target, pname, parameters );
//    }

    /// <inheritdoc cref="Gl.TexParameteri{T}(TextureTarget,TextureParameterName,T)" />
    public void GLTexParameteri<T>( TextureTarget target, TextureParameterName pname, T param ) where T : struct => Gl.TexParameteri( target, pname, param );

    //    public void GLTexParameteriv( int target, int pname, IntBuffer parameters )
//    {
//        Gl.TexParameteriv( target, pname, parameters );
//    }

    public void GLTexSubImage2D( TextureTarget target,
                                 int level,
                                 int xoffset,
                                 int yoffset,
                                 int width,
                                 int height,
                                 PixelFormat format,
                                 PixelType type,
                                 Buffer pixels )
    {
        switch ( pixels )
        {
            case ByteBuffer buffer:
                Gl.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, buffer );

                break;

            case ShortBuffer buffer:
                Gl.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, buffer );

                break;

            case IntBuffer buffer:
                Gl.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, buffer );

                break;

            case FloatBuffer buffer:
                Gl.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, buffer );

                break;

            case DoubleBuffer buffer:
                Gl.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, buffer );

                break;

            default:
                throw new GdxRuntimeException( $"Can't use {pixels.GetType().Name} with this method. "
                                             + $"Use ByteBuffer, ShortBuffer, IntBuffer, FloatBuffer "
                                             + $"or DoubleBuffer instead." );
        }
    }

    /// <inheritdoc cref="Gl.Uniform1f{T}(int,int,T)" />
    public void GLUniform1F<T>( int location, int count, T value ) where T : struct => Gl.Uniform1f( location, count, value );

    //    public void GLUniform1Fv( int location, int count, FloatBuffer v )
//    {
//        Gl.Uniform1fv( location, v );
//    }

//    public void GLUniform1Fv( int location, int count, float[] v, int offset )
//    {
//        Gl.Uniform1fv( location, toFloatBuffer( v, offset, count ) );
//    }

    /// <inheritdoc cref="Gl.Uniform1i{T}(int,int,T)" />
    public void GLUniform1I<T>( int location, int count, T value ) where T : struct => Gl.Uniform1i( location, count, value );

    //    public void GLUniform1Iv( int location, int count, IntBuffer v )
//    {
//        Gl.Uniform1iv( location, v );
//    }

//    public void GLUniform1Iv( int location, int count, int[] v, int offset )
//    {
//        Gl.Uniform1iv( location, toIntBuffer( v, offset, count ) );
//    }

    /// <inheritdoc cref="Gl.Uniform2f{T}(int,int,T)" />
    public void GLUniform2F<T>( int location, int count, T value ) where T : struct => Gl.Uniform2f( location, count, value );

    //    public void GLUniform2Fv( int location, int count, FloatBuffer v )
//    {
//        Gl.Uniform2fv( location, v );
//    }

//    public void GLUniform2Fv( int location, int count, float[] v, int offset )
//    {
//        Gl.Uniform2fv( location, toFloatBuffer( v, offset, count << 1 ) );
//    }

    /// <inheritdoc cref="Gl.Uniform2i{T}(int,int,T)" />
    public void GLUniform2I<T>( int location, int count, T value ) where T : struct => Gl.Uniform2i( location, count, value );

    //    public void GLUniform2Iv( int location, int count, IntBuffer v )
//    {
//        Gl.Uniform2iv( location, v );
//    }

//    public void GLUniform2Iv( int location, int count, int[] v, int offset )
//    {
//        Gl.Uniform2iv( location, toIntBuffer( v, offset, count << 1 ) );
//    }

    /// <inheritdoc cref="Gl.Uniform3f{T}(int,int,T)" />
    public void GLUniform3F<T>( int location, int count, T value ) where T : struct => Gl.Uniform3f( location, count, value );

    //    public void GLUniform3Fv( int location, int count, FloatBuffer v )
//    {
//        Gl.Uniform3fv( location, v );
//    }

//    public void GLUniform3Fv( int location, int count, float[] v, int offset )
//    {
//        Gl.Uniform3fv( location, toFloatBuffer( v, offset, count * 3 ) );
//    }

    /// <inheritdoc cref="Gl.Uniform3i{T}(int,int,T)" />
    public void GLUniform3I<T>( int location, int count, T value ) where T : struct => Gl.Uniform3i( location, count, value );

    //    public void GLUniform3Iv( int location, int count, IntBuffer v )
//    {
//        Gl.Uniform3iv( location, v );
//    }

//    public void GLUniform3Iv( int location, int count, int[] v, int offset )
//    {
//        Gl.Uniform3iv( location, ToIntBuffer( v, offset, count * 3 ) );
//    }

    /// <summary>
    ///     Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">
    ///     Specifies the location of the uniform variable to be modified.
    /// </param>
    /// <param name="count">
    ///     For the vector (Gl.Uniform*v) commands, specifies the number of elements that are to be
    ///     modified. This should be 1 if the targeted uniform variable is not an array, and 1 or
    ///     more if it is an array.
    /// </param>
    /// <param name="value">
    ///     For the vector and matrix commands, specifies a pointer to an array of <paramref name="count" />
    ///     values that will be used to update the specified uniform variable.
    /// </param>
    public void GLUniform4F<T>( int location, int count, T value ) where T : struct => Gl.Uniform4f( location, count, value );

    //    public void GLUniform4Fv( int location, int count, FloatBuffer v )
//    {
//        Gl.Uniform4fv( location, v );
//    }

//    public void GLUniform4Fv( int location, int count, float[] v, int offset )
//    {
//        Gl.Uniform4fv( location, ToFloatBuffer( v, offset, count << 2 ) );
//    }

    /// <inheritdoc cref="Gl.Uniform4i{T}(int,int,T)" />
    public void GLUniform4I<T>( int location, int count, T value ) where T : struct => Gl.Uniform4i( location, count, value );

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

    /// <inheritdoc cref="Gl.UseProgram(uint)" />
    public void GLUseProgram( uint program ) => Gl.UseProgram( program );

    /// <inheritdoc cref="Gl.ValidateProgram(uint)" />
    public void GLValidateProgram( uint program ) => Gl.ValidateProgram( program );

    /// <inheritdoc cref="Gl.VertexAttrib1f{T}(uint,T)" />
    public void GLVertexAttrib1F<T>( uint indx, T x ) where T : struct => Gl.VertexAttrib1f( indx, x );

    /// <inheritdoc cref="Gl.VertexAttrib1f{T}(uint,T)" />
    public void GLVertexAttrib1Fv<T>( uint indx, T values ) where T : struct => Gl.VertexAttrib1f( indx, values );

    //    public void GLVertexAttrib2F( int indx, float x, float y )
//    {
//        //TODO:
//        throw new NotImplementedException();
//    }

    /// <inheritdoc cref="Gl.VertexAttrib2f{T}(uint,T)" />
    public void GLVertexAttrib2Fv<T>( uint indx, T values ) where T : struct => Gl.VertexAttrib2f( indx, values );

    //    public void GLVertexAttrib3F( int indx, float x, float y, float z )
//    {
//        //TODO:
//        throw new NotImplementedException();
//    }

    /// <inheritdoc cref="Gl.VertexAttrib3f{T}(uint,T)" />
    public void GLVertexAttrib3Fv<T>( uint indx, T values ) where T : struct => Gl.VertexAttrib3f( indx, values );

    //    public void GLVertexAttrib4F( int indx, float x, float y, float z, float w )
//    {
//        //TODO:
//        throw new NotImplementedException();
//    }

    /// <inheritdoc cref="Gl.VertexAttrib4f{T}(uint,T)" />
    public void GLVertexAttrib4Fv<T>( uint indx, T values ) where T : struct => Gl.VertexAttrib4f( indx, values );

    /// <summary>
    ///     define an array of generic vertex attribute data.
    /// </summary>
    /// <param name="indx">
    ///     Specifies the index of the generic vertex attribute to be modified.
    /// </param>
    /// <param name="size">
    ///     Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4.
    ///     Additionally, the symbolic constant Gl.BGRA is accepted by Gl.VertexAttribPointer.
    ///     The initial value is 4.
    /// </param>
    /// <param name="type">
    ///     Specifies the data type of each component in the array. The symbolic constants Gl.BYTE,
    ///     Gl.UNSIGNED_BYTE, Gl.SHORT, Gl.UNSIGNED_SHORT, Gl.INT, and Gl.UNSIGNED_INT are accepted
    ///     by Gl.VertexAttribPointer and Gl.VertexAttribIPointer. Additionally Gl.HALF_FLOAT, Gl.FLOAT,
    ///     Gl.DOUBLE, Gl.FIXED, Gl.INT_2_10_10_10_REV, Gl.UNSIGNED_INT_2_10_10_10_REV and
    ///     Gl.UNSIGNED_INT_10F_11F_11F_REV are accepted by Gl.VertexAttribPointer. Gl.DOUBLE is also
    ///     accepted by Gl.VertexAttribLPointer and is the only token accepted by the type parameter
    ///     for that function. The initial value is Gl.FLOAT.
    /// </param>
    /// <param name="normalized">
    ///     For Gl.VertexAttribPointer, specifies whether fixed-point data values should be normalized
    ///     (Gl.TRUE) or converted directly as fixed-point values (Gl.FALSE) when they are accessed.
    /// </param>
    /// <param name="stride">
    ///     Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the
    ///     generic vertex attributes are understood to be tightly packed in the array.
    ///     The initial value is 0.
    /// </param>
    /// <param name="pointer">
    ///     Specifies a offset of the first component of the first generic vertex attribute in the array
    ///     in the data store of the buffer currently bound to the Gl.ARRAY_BUFFER target.
    ///     The initial value is 0.
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
            switch ( type )
            {
                case VertexAttribType.Byte:
                case VertexAttribType.UnsignedByte:
                    Gl.VertexAttribPointer( indx, size, type, normalized, stride, byteBuffer );

                    break;

                case VertexAttribType.Short:
                case VertexAttribType.UnsignedShort:
                    Gl.VertexAttribPointer( indx, size, type, normalized, stride, byteBuffer.AsShortBuffer() );

                    break;

                case VertexAttribType.Float:
                    Gl.VertexAttribPointer( indx, size, type, normalized, stride, byteBuffer.AsFloatBuffer() );

                    break;

                default:
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

    /// <inheritdoc cref="Gl.DrawElements(PrimitiveType,int,DrawElementsType,IntPtr)" />
    public void GLDrawElements( PrimitiveType mode, int count, DrawElementsType type, IntPtr indices ) => Gl.DrawElements( mode, count, type, indices );

    /// <inheritdoc cref="Gl.VertexAttribPointer(uint,int,VertexAttribType,bool,int,IntPtr)" />
    public void GLVertexAttribPointer( uint indx,
                                       int size,
                                       VertexAttribType type,
                                       bool normalized,
                                       int stride,
                                       IntPtr ptr ) => Gl.VertexAttribPointer( indx, size, type, normalized, stride, ptr );
}
