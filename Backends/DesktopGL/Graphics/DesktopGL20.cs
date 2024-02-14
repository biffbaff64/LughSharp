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


using System.Text;

using LibGDXSharp.LibCore.Graphics;
using LibGDXSharp.LibCore.Utils;
using LibGDXSharp.LibCore.Utils.Buffers;

using Buffer = LibGDXSharp.LibCore.Utils.Buffers.Buffer;

namespace LibGDXSharp.Backends.DesktopGL.Graphics;

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

//    public void GLRenderbufferStorage( int target, int int, int width, int height )
//    {
//        EXTFramebufferObject.glRenderbufferStorageEXT( target, int, width, height );
//    }

    /// <inheritdoc cref="Gl.SampleCoverage(float,bool)" />
    public void GLSampleCoverage( float value, bool invert ) => Gl.SampleCoverage( value, invert );

    /// <inheritdoc cref="Gl.Scissor(int,int,int,int)" />
    public void GLScissor( int x, int y, int width, int height ) => Gl.Scissor( x, y, width, height );

    /// <inheritdoc cref="Gl.Viewport(int,int,int,int)" />
    public void GLViewport( int x, int y, int width, int height ) => Gl.Viewport( x, y, width, height );

    /// <inheritdoc cref="Gl.ActiveTexture(int)" />
    public void GLActiveTexture( int texture ) => Gl.ActiveTexture( texture );

    /// <inheritdoc cref="Gl.AttachShader(int,int)" />
    public void GLAttachShader( int program, int shader ) => Gl.AttachShader( program, shader );

    /// <inheritdoc cref="Gl.BindTexture(int,int)" />
    public void GLBindTexture( int target, int texture ) => Gl.BindTexture( target, texture );

    /// <inheritdoc cref="Gl.BlendFunc(int,int)" />
    public void GLBlendFunc( int sfactor, int dfactor ) => Gl.BlendFunc( sfactor, dfactor );

    /// <inheritdoc cref="Gl.CreateProgram()" />
    public int GLCreateProgram() => Gl.CreateProgram();

    /// <inheritdoc cref="Gl.DeleteBuffers(int[])" />
    public void GLDeleteBuffers( params int[] buffers ) => Gl.DeleteBuffers( buffers );

    //    public void GLDeleteFramebuffers( int n, IntBuffer framebuffers )
//    {
//        EXTFramebufferObject.glDeleteFramebuffersEXT( framebuffers );
//    }

//    public void GLDeleteFramebuffer( int framebuffer )
//    {
//        EXTFramebufferObject.glDeleteFramebuffersEXT( framebuffer );
//    }

    /// <inheritdoc cref="Gl.DeleteProgram(int)" />
    public void GLDeleteProgram( int program ) => Gl.DeleteProgram( program );

    /// <inheritdoc cref="Gl.DeleteTextures(int[])" />
    public void GLDeleteTextures( params int[] textures ) => Gl.DeleteTextures( textures );

    /// <inheritdoc cref="Gl.GenBuffer()" />
    public int GLGenBuffer() => GL.GenBuffer();

    /// <inheritdoc cref="Gl.GenTexture()" />
    public int GLGenTexture() => Gl.GenTexture();

    /// <inheritdoc cref="Gl.GetAttribLocation(int,string)" />
    public int GLGetAttribLocation( int program, string name ) => Gl.GetAttribLocation( program, name );

    //    public void GLGetBooleanv( int pname, Buffer parameters )
//    {
//        Gl.GetBooleanv( pname, ( ByteBuffer )parameters );
//    }

//    public void GLGetBufferParameteriv( int target, int pname, IntBuffer parameters )
//    {
//        Gl.GetBufferParameteriv( target, pname, parameters );
//    }

    /// <inheritdoc cref="Gl.GetError()" />
    public int GLGetError() => Gl.GetError();

    //    public void GLGetTexParameterfv( int target, int pname, FloatBuffer parameters )
//    {
//        Gl.GetTexParameterfv( target, pname, parameters );
//    }

//    public void GLGetTexParameteriv( int target, int pname, IntBuffer parameters )
//    {
//        Gl.GetTexParameteriv( target, pname, parameters );
//    }

    /// <inheritdoc cref="Gl.GetUniformLocation(int,string)" />
    public int GLGetUniformLocation( int program, string name ) => Gl.GetUniformLocation( program, name );

    /// <inheritdoc cref="Gl.LinkProgram(int)" />
    public void GLLinkProgram( int program ) => Gl.LinkProgram( program );

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

    /// <inheritdoc cref="Gl.UseProgram(int)" />
    public void GLUseProgram( int program ) => Gl.UseProgram( program );

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

    /// <inheritdoc cref="Gl.BindAttribLocation(int,int,string)" />
    public void GLBindAttribLocation( int program, int index, string name ) => Gl.BindAttribLocation( program, index, name );

    /// <inheritdoc cref="Gl.BindBuffer(int,int)" />
    public void GLBindBuffer( int target, int buffer ) => Gl.BindBuffer( target, buffer );

    /// <inheritdoc cref="Gl.BindFramebufferEXT(int,int)" />
    public void GLBindFramebuffer( int target, int framebuffer ) => Gl.BindFramebufferEXT( target, framebuffer );

    /// <inheritdoc cref="Gl.BindRenderbufferEXT(int,int)" />
    public void GLBindRenderbuffer( int target, int renderbuffer ) => Gl.BindRenderbufferEXT( target, renderbuffer );

    /// <inheritdoc cref="Gl.BlendEquation(int)" />
    public void GLBlendEquation( int mode ) => Gl.BlendEquation( mode );

    /// <inheritdoc cref="Gl.BlendFuncSeparate(int,int,int,int)" />
    public void GLBlendEquationSeparate( int modeRGB, int modeAlpha ) => Gl.BlendEquationSeparate( modeRGB, modeAlpha );

    /// <inheritdoc cref="Gl.BlendFuncSeparate(int,int,int,int)" />
    public void GLBlendFuncSeparate( int srcRGB,
                                     int dstRGB,
                                     int srcAlpha,
                                     int dstAlpha ) => Gl.BlendFuncSeparate( srcRGB, dstRGB, srcAlpha, dstAlpha );

    public void GLBufferData( int target,
                              int size,
                              Buffer? data,
                              int usage )
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

    public void GLBufferSubData( int target, IntPtr offset, int size, Buffer data )
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

    /// <inheritdoc cref="Gl.Clear(int)" />
    public void GLClear( int mask ) => Gl.Clear( mask );

    /// <inheritdoc cref="Gl.CompileShader(int)" />
    public void GLCompileShader( int shader ) => Gl.CompileShader( shader );

    public void GLCompressedTexImage2D( int target,
                                        int level,
                                        int int,
                                        int width,
                                        int height,
                                        int border,
                                        int imageSize,
                                        Buffer data )
    {
        if ( data is ByteBuffer buffer )
        {
            Gl.CompressedTexImage2D( target, level, int, width, height, border, imageSize, buffer );
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

    /// <inheritdoc cref="Gl.CopyTexImage2D(int,int,int,int,int,int,int,int)" />
    public void GLCopyTexImage2D( int target,
                                  int level,
                                  int int,
                                  int x,
                                  int y,
                                  int width,
                                  int height,
                                  int border ) => Gl.CopyTexImage2D( target, level, int, x, y, width, height, border );

    /// <inheritdoc cref="Gl.CopyTexSubImage2D(int,int,int,int,int,int,int,int)" />
    public void GLCopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height )
        => Gl.CopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );

    /// <inheritdoc cref="Gl.CreateShader(int)" />
    public int GLCreateShader( int type ) => Gl.CreateShader( type );

    /// <inheritdoc cref="Gl.CullFace(int)" />
    public void GLCullFace( int mode ) => Gl.CullFace( mode );

    //    public void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers )
//    {
//        EXTFramebufferObject.glDeleteRenderbuffersEXT( renderbuffers );
//    }

//    public void GLDeleteRenderbuffer( int renderbuffer )
//    {
//        EXTFramebufferObject.glDeleteRenderbuffersEXT( renderbuffer );
//    }

    /// <inheritdoc cref="Gl.DeleteShader(int)" />
    public void GLDeleteShader( int shader ) => Gl.DeleteShader( shader );

    /// <inheritdoc cref="Gl.DepthFunc(int)" />
    public void GLDepthFunc( int func ) => Gl.DepthFunc( func );

    public void GLDetachShader( int program, int shader ) => Gl.DetachShader( program, shader );

    public void GLDisable( int cap ) => Gl.Disable( cap );

    public void GLDisableVertexAttribArray( int index ) => Gl.DisableVertexAttribArray( index );

    public void GLDrawArrays( int mode, int first, int count ) => Gl.DrawArrays( mode, first, count );

    public void GLDrawElements( int mode, int count, int type, Buffer indices )
    {
        if ( indices is ShortBuffer buffer && ( type == int.UnsignedShort ) )
        {
            Gl.DrawElements( mode, count, type, buffer );
        }
        else if ( indices is ByteBuffer byteBuf1 && ( type == int.UnsignedShort ) )
        {
            Gl.DrawElements( mode, count, type, byteBuf1.AsShortBuffer() );
        }
        else if ( indices is ByteBuffer byteBuf2 && ( type == int.UnsignedByte ) )
        {
            Gl.DrawElements( mode, count, type, byteBuf2 );
        }
        else
        {
            throw new GdxRuntimeException( $"Can't use {indices.GetType().Name} with this "
                                         + $"method. Use ShortBuffer or ByteBuffer instead." );
        }
    }

    /// <inheritdoc cref="Gl.Enable(int)" />
    public void GLEnable( int cap ) => Gl.Enable( cap );

    /// <inheritdoc cref="Gl.EnableVertexAttribArray(int)" />
    public void GLEnableVertexAttribArray( int index ) => Gl.EnableVertexAttribArray( index );

    //    public void GLFramebufferRenderbuffer( int target, int attachment, int int, int renderbuffer )
//    {
//        EXTFramebufferObject.glFramebufferRenderbufferEXT( target, attachment, int, renderbuffer );
//    }

//    public void GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level )
//    {
//        EXTFramebufferObject.glFramebufferTexture2DEXT( target, attachment, textarget, texture, level );
//    }

    /// <inheritdoc cref="Gl.FrontFace(int)" />
    public void GLFrontFace( int mode ) => Gl.FrontFace( mode );

    /// <inheritdoc cref="Gl.GenBuffers(int[])" />
    public void GLGenBuffers( int[] buffers ) => Gl.GenBuffers( buffers );

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

    /// <inheritdoc cref="Gl.GenTextures(int[])" />
    public void GLGenTextures( int[] textures ) => Gl.GenTextures( textures );

    //    public void GLGenerateMipmap( int target )
//    {
//        EXTFramebufferObject.glGenerateMipmapEXT( target );
//    }

    /// <inheritdoc cref="Gl.GetActiveAttrib(int,int,int,out int,out int,out int,StringBuilder)" />
    public void GLGetActiveAttrib( int program,
                                   int index,
                                   int bufSize,
                                   out int length,
                                   out int size,
                                   out int type,
                                   StringBuilder name ) => Gl.GetActiveAttrib( program, index, bufSize, out length, out size, out type, name );

    /// <inheritdoc cref="Gl.GetActiveUniform(int,int,int,out int, out int,out int,StringBuilder)" />
    public void GLGetActiveUniform( int program,
                                    int index,
                                    int bufSize,
                                    out int length,
                                    out int size,
                                    out int type,
                                    StringBuilder name ) => Gl.GetActiveUniform( program, index, bufSize, out length, out size, out type, name );

    /// <inheritdoc cref="Gl.GetAttachedShaders(int,out int,int[])" />
    public void GLGetAttachedShaders( int program, out int buffer, int[] shaders ) => Gl.GetAttachedShaders( program, out buffer, shaders );

    public void GLGetFloatv<T>( int pname, out T parameters ) where T : struct => Gl.GetFloat( pname, out parameters );

    //    public void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters )
//    {
//        EXTFramebufferObject.glGetFramebufferAttachmentParameterivEXT( target, attachment, pname, parameters );
//    }

//    public void GLGetIntegerv( int pname, IntBuffer parameters )
//    {
//        Gl.GetIntegerv( pname, parameters );
//    }

//    public string GLGetProgramInfoLog( int program )
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

//    public void GLGetShaderPrecisionFormat( int int, int precisiontype, IntBuffer range, IntBuffer precision )
//    {
//        throw new UnsupportedOperationException( "unsupported, won't implement" );
//    }

//    public void GLGetShaderiv( int shader, int pname, IntBuffer parameters )
//    {
//        Gl.GetShaderiv( shader, pname, parameters );
//    }

    /// <inheritdoc cref="Gl.GetString(int)" />
    public string GLGetString( int name ) => Gl.GetString( name );

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

    /// <inheritdoc cref="Gl.Hint(int,int)" />
    public void GLHint( int target, int mode ) => Gl.Hint( target, mode );

    /// <inheritdoc cref="Gl.IsBuffer(int)" />
    public bool GLIsBuffer( int buffer ) => Gl.IsBuffer( buffer );

    /// <inheritdoc cref="Gl.IsEnabled(int)" />
    public bool GLIsEnabled( int cap ) => Gl.IsEnabled( cap );

    //    public bool GLIsFramebuffer( int framebuffer )
//    {
//        return EXTFramebufferObject.glIsFramebufferEXT( framebuffer );
//    }

    /// <inheritdoc cref="Gl.IsProgram(int)" />
    public bool GLIsProgram( int program ) => Gl.IsProgram( program );

    //    public bool GLIsRenderbuffer( int renderbuffer )
//    {
//        return EXTFramebufferObject.glIsRenderbufferEXT( renderbuffer );
//    }

    /// <inheritdoc cref="Gl.IsShader(int)" />
    public bool GLIsShader( int shader ) => Gl.IsShader( shader );

    /// <inheritdoc cref="Gl.IsTexture(int)" />
    public bool GLIsTexture( int texture ) => Gl.IsTexture( texture );

    public void GLPixelStorei( int pname, int param ) => Gl.PixelStore( pname, param );

    /// <inheritdoc cref="Gl.ReadPixels(int,int,int,int,int,int,IntPtr)" />
    public void GLReadPixels( int x, int y, int width, int height, int format, int type, IntPtr pixels )
        => Gl.ReadPixels( x, y, width, height, format, type, pixels );

    //    public void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length )
//    {
//        throw new UnsupportedOperationException( "unsupported, won't implement" );
//    }

    /// <inheritdoc cref="Gl.ShaderSource(int,string[])" />
    public void GLShaderSource( int shader, string[] str ) => Gl.ShaderSource( shader, str );

    /// <inheritdoc cref="Gl.StencilFunc(int,int,int)" />
    public void GLStencilFunc( int func, int reference, int mask ) => Gl.StencilFunc( func, reference, mask );

    /// <inheritdoc cref="Gl.StencilFuncSeparate(int,int,int,int)" />
    public void GLStencilFuncSeparate( int face, int func, int reference, int mask )
        => Gl.StencilFuncSeparate( face, func, reference, mask );

    /// <inheritdoc cref="Gl.StencilMask(int)" />
    public void GLStencilMask( int mask ) => Gl.StencilMask( mask );

    /// <inheritdoc cref="Gl.StencilMaskSeparate(int,int)" />
    public void GLStencilMaskSeparate( int face, int mask ) => Gl.StencilMaskSeparate( face, mask );

    /// <inheritdoc cref="Gl.int(int,int,int)" />
    public void GLint( int fail, int zfail, int zpass ) => Gl.int( fail, zfail, zpass );

    /// <inheritdoc cref="Gl.intSeparate(int,int,int,int)" />
    public void GLintSeparate( int face, int fail, int zfail, int zpass )
        => Gl.intSeparate( face, fail, zfail, zpass );

    public void GLTexImage2D( int target,
                              int level,
                              int int,
                              int width,
                              int height,
                              int border,
                              int format,
                              int type,
                              IntPtr pixels ) => Gl.TexImage2D( target, level, int, width, height, border, format, type, pixels );

    /// <inheritdoc cref="Gl.TexParameterf{T}(int,int,T)" />
    public void GLTexParameterf<T>( int target, int pname, T param ) where T : struct => Gl.TexParameterf( target, pname, param );

    //    public void GLTexParameterfv( int target, int pname, FloatBuffer parameters )
//    {
//        Gl.TexParameterfv( target, pname, parameters );
//    }

    /// <inheritdoc cref="Gl.TexParameteri{T}(int,int,T)" />
    public void GLTexParameteri<T>( int target, int pname, T param ) where T : struct => Gl.TexParameteri( target, pname, param );

    //    public void GLTexParameteriv( int target, int pname, IntBuffer parameters )
//    {
//        Gl.TexParameteriv( target, pname, parameters );
//    }

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

    /// <inheritdoc cref="Gl.ValidateProgram(int)" />
    public void GLValidateProgram( int program ) => Gl.ValidateProgram( program );

    /// <inheritdoc cref="Gl.VertexAttrib1f{T}(int,T)" />
    public void GLVertexAttrib1F<T>( int indx, T x ) where T : struct => Gl.VertexAttrib1f( indx, x );

    /// <inheritdoc cref="Gl.VertexAttrib1f{T}(int,T)" />
    public void GLVertexAttrib1Fv<T>( int indx, T values ) where T : struct => Gl.VertexAttrib1f( indx, values );

    //    public void GLVertexAttrib2F( int indx, float x, float y )
//    {
//        //TODO:
//        throw new NotImplementedException();
//    }

    /// <inheritdoc cref="Gl.VertexAttrib2f{T}(int,T)" />
    public void GLVertexAttrib2Fv<T>( int indx, T values ) where T : struct => Gl.VertexAttrib2f( indx, values );

    //    public void GLVertexAttrib3F( int indx, float x, float y, float z )
//    {
//        //TODO:
//        throw new NotImplementedException();
//    }

    /// <inheritdoc cref="Gl.VertexAttrib3f{T}(int,T)" />
    public void GLVertexAttrib3Fv<T>( int indx, T values ) where T : struct => Gl.VertexAttrib3f( indx, values );

    //    public void GLVertexAttrib4F( int indx, float x, float y, float z, float w )
//    {
//        //TODO:
//        throw new NotImplementedException();
//    }

    /// <inheritdoc cref="Gl.VertexAttrib4f{T}(int,T)" />
    public void GLVertexAttrib4Fv<T>( int indx, T values ) where T : struct => Gl.VertexAttrib4f( indx, values );

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
    public void GLVertexAttribPointer( int indx,
                                       int size,
                                       int type,
                                       bool normalized,
                                       int stride,
                                       Buffer pointer )
    {
        if ( pointer is ByteBuffer byteBuffer )
        {
            switch ( type )
            {
                case int.Byte:
                case int.UnsignedByte:
                    Gl.VertexAttribPointer( indx, size, type, normalized, stride, byteBuffer );

                    break;

                case int.Short:
                case int.UnsignedShort:
                    Gl.VertexAttribPointer( indx, size, type, normalized, stride, byteBuffer.AsShortBuffer() );

                    break;

                case int.Float:
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
            if ( type == int.Float )
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

    /// <inheritdoc cref="Gl.DrawElements(int,int,int,IntPtr)" />
    public void GLDrawElements( int mode, int count, int type, IntPtr indices ) => Gl.DrawElements( mode, count, type, indices );

    /// <inheritdoc cref="Gl.VertexAttribPointer(int,int,int,bool,int,IntPtr)" />
    public void GLVertexAttribPointer( int indx,
                                       int size,
                                       int type,
                                       bool normalized,
                                       int stride,
                                       IntPtr ptr ) => Gl.VertexAttribPointer( indx, size, type, normalized, stride, ptr );
}
