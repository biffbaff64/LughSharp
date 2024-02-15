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

    //    public void GLReleaseShaderCompiler()
//    {
//        // nothing to do here
//    }

//    public void GLRenderbufferStorage( int target, int int, int width, int height )
//    {
//        EXTFramebufferObject.glRenderbufferStorageEXT( target, int, width, height );
//    }

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

    /// <inheritdoc cref="GL.glDeleteBuffers(uint[])" />
    public void GLDeleteBuffers( params uint[] buffers )
    {
        GL.glDeleteBuffers( buffers );
    }

    //    public void GLDeleteFramebuffers( int n, IntBuffer framebuffers )
//    {
//        EXTFramebufferObject.glDeleteFramebuffersEXT( framebuffers );
//    }

//    public void GLDeleteFramebuffer( int framebuffer )
//    {
//        EXTFramebufferObject.glDeleteFramebuffersEXT( framebuffer );
//    }

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

    //    public void GLGetBooleanv( int pname, Buffer parameters )
//    {
//        GL.glGetBooleanv( pname, ( ByteBuffer )parameters );
//    }

//    public void GLGetBufferParameteriv( int target, int pname, IntBuffer parameters )
//    {
//        GL.glGetBufferParameteriv( target, pname, parameters );
//    }

    /// <inheritdoc cref="GL.glGetError()" />
    public int GLGetError()
    {
        return GL.glGetError();
    }

    //    public void GLGetTexParameterfv( int target, int pname, FloatBuffer parameters )
//    {
//        GL.glGetTexParameterfv( target, pname, parameters );
//    }

//    public void GLGetTexParameteriv( int target, int pname, IntBuffer parameters )
//    {
//        GL.glGetTexParameteriv( target, pname, parameters );
//    }

    /// <inheritdoc cref="GL.glGetUniformLocation(int,string)" />
    public int GLGetUniformLocation( int program, string name )
    {
        return GL.glGetUniformLocation( program, name );
    }

    /// <inheritdoc cref="GL.glLinkProgram(int)" />
    public void GLLinkProgram( int program )
    {
        GL.glLinkProgram( program );
    }

    //    public void GLUniform4Iv( int location, int count, IntBuffer v )
//    {
//        GL.glUniform4iv( location, v );
//    }

//    public void GLUniform4Iv( int location, int count, int[] v, int offset )
//    {
//        GL.glUniform4iv( location, toIntBuffer( v, offset, count << 2 ) );
//    }

//    public void GLUniformMatrix2Fv( int location, int count, bool transpose, FloatBuffer value )
//    {
//        GL.glUniformMatrix2fv( location, transpose, value );
//    }

//    public void GLUniformMatrix2Fv( int location, int count, bool transpose, float[] value, int offset )
//    {
//        GL.glUniformMatrix2fv( location, transpose, ToFloatBuffer( value, offset, count << 2 ) );
//    }

//    public void GLUniformMatrix3Fv( int location, int count, bool transpose, FloatBuffer value )
//    {
//        GL.glUniformMatrix3fv( location, transpose, value );
//    }

//    public void GLUniformMatrix3Fv( int location, int count, bool transpose, float[] value, int offset )
//    {
//        GL.glUniformMatrix3fv( location, transpose, ToFloatBuffer( value, offset, count * 9 ) );
//    }

//    public void GLUniformMatrix4Fv( int location, int count, bool transpose, FloatBuffer value )
//    {
//        GL.glUniformMatrix4fv( location, transpose, value );
//    }

//    public void GLUniformMatrix4Fv( int location, int count, bool transpose, float[] value, int offset )
//    {
//        GL.glUniformMatrix4fv( location, transpose, ToFloatBuffer( value, offset, count << 4 ) );
//    }

    /// <inheritdoc cref="GL.glUseProgram(int)" />
    public void GLUseProgram( int program )
    {
        GL.glUseProgram( program );
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

    /// <inheritdoc cref="GL.glBindAttribLocation(int,int,string)" />
    public void GLBindAttribLocation( int program, int index, string name )
    {
        GL.glBindAttribLocation( program, index, name );
    }

    /// <inheritdoc cref="GL.glBindBuffer(int,int)" />
    public void GLBindBuffer( int target, int buffer )
    {
        GL.glBindBuffer( target, buffer );
    }

    /// <inheritdoc cref="GL.glBindFramebufferEXT(int,int)" />
    public void GLBindFramebuffer( int target, int framebuffer )
    {
        GL.glBindFramebufferEXT( target, framebuffer );
    }

    /// <inheritdoc cref="GL.glBindRenderbufferEXT(int,int)" />
    public void GLBindRenderbuffer( int target, int renderbuffer )
    {
        GL.glBindRenderbufferEXT( target, renderbuffer );
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

    public void GLBufferData( int target,
                              int size,
                              Buffer? data,
                              int usage )
    {
        switch ( data )
        {
            case null:
                GL.glBufferData( target, size, data, usage );

                break;

            case ByteBuffer byteBuffer:
                GL.glBufferData( target, size, byteBuffer, usage );

                break;

            case IntBuffer intBuffer:
                GL.glBufferData( target, size, intBuffer, usage );

                break;

            case FloatBuffer floatBuffer:
                GL.glBufferData( target, size, floatBuffer, usage );

                break;

            case DoubleBuffer doubleBuffer:
                GL.glBufferData( target, size, doubleBuffer, usage );

                break;

            case ShortBuffer shortBuffer:
                GL.glBufferData( target, size, shortBuffer, usage );

                break;
        }
    }

    public void GLBufferSubData( int target, IntPtr offset, int size, Buffer data )
    {
        ArgumentNullException.ThrowIfNull( data );

        switch ( data )
        {
            case ByteBuffer byteBuffer:
                GL.glBufferSubData( target, offset, size, byteBuffer );

                break;

            case IntBuffer intBuffer:
                GL.glBufferSubData( target, offset, size, intBuffer );

                break;

            case FloatBuffer floatBuffer:
                GL.glBufferSubData( target, offset, size, floatBuffer );

                break;

            case DoubleBuffer doubleBuffer:
                GL.glBufferSubData( target, offset, size, doubleBuffer );

                break;

            case ShortBuffer shortBuffer:
                GL.glBufferSubData( target, offset, size, shortBuffer );

                break;
        }
    }

//    public int GLCheckFramebufferStatus( int target )
//    {
//        return EXTFramebufferObject.glCheckFramebufferStatusEXT( target );
//    }

    /// <inheritdoc cref="GL.glClear(int)" />
    public void GLClear( int mask )
    {
        GL.glClear( mask );
    }

    /// <inheritdoc cref="GL.glCompileShader(int)" />
    public void GLCompileShader( int shader )
    {
        GL.glCompileShader( shader );
    }

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
            GL.glCompressedTexImage2D( target, level, int, width, height, border, imageSize, buffer );
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

    /// <inheritdoc cref="GL.glCopyTexImage2D(int,int,int,int,int,int,int,int)" />
    public void GLCopyTexImage2D( int target,
                                  int level,
                                  int int,
                                  int x,
                                  int y,
                                  int width,
                                  int height,
                                  int border ) => GL.glCopyTexImage2D( target, level, int, x, y, width, height, border );

    /// <inheritdoc cref="GL.glCopyTexSubImage2D(int,int,int,int,int,int,int,int)" />
    public void GLCopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
        GL.glCopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );
    }

    /// <inheritdoc cref="GL.glCreateShader(int)" />
    public int GLCreateShader( int type )
    {
        return GL.glCreateShader( type );
    }

    /// <inheritdoc cref="GL.glCullFace(int)" />
    public void GLCullFace( int mode )
    {
        GL.glCullFace( mode );
    }

    //    public void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers )
//    {
//        EXTFramebufferObject.glDeleteRenderbuffersEXT( renderbuffers );
//    }

//    public void GLDeleteRenderbuffer( int renderbuffer )
//    {
//        EXTFramebufferObject.glDeleteRenderbuffersEXT( renderbuffer );
//    }

    /// <inheritdoc cref="GL.glDeleteShader(int)" />
    public void GLDeleteShader( int shader )
    {
        GL.glDeleteShader( shader );
    }

    /// <inheritdoc cref="GL.glDepthFunc(int)" />
    public void GLDepthFunc( int func )
    {
        GL.glDepthFunc( func );
    }

    public void GLDetachShader( int program, int shader )
    {
        GL.glDetachShader( program, shader );
    }

    public void GLDisable( int cap )
    {
        GL.glDisable( cap );
    }

    public void GLDisableVertexAttribArray( int index )
    {
        GL.glDisableVertexAttribArray( index );
    }

    public void GLDrawArrays( int mode, int first, int count )
    {
        GL.glDrawArrays( mode, first, count );
    }

    public void GLDrawElements( int mode, int count, int type, Buffer indices )
    {
        if ( indices is ShortBuffer buffer && ( type == int.UnsignedShort ) )
        {
            GL.glDrawElements( mode, count, type, buffer );
        }
        else if ( indices is ByteBuffer byteBuf1 && ( type == int.UnsignedShort ) )
        {
            GL.glDrawElements( mode, count, type, byteBuf1.AsShortBuffer() );
        }
        else if ( indices is ByteBuffer byteBuf2 && ( type == int.UnsignedByte ) )
        {
            GL.glDrawElements( mode, count, type, byteBuf2 );
        }
        else
        {
            throw new GdxRuntimeException( $"Can't use {indices.GetType().Name} with this "
                                         + $"method. Use ShortBuffer or ByteBuffer instead." );
        }
    }

    /// <inheritdoc cref="GL.glEnable(int)" />
    public void GLEnable( int cap )
    {
        GL.glEnable( cap );
    }

    /// <inheritdoc cref="GL.glEnableVertexAttribArray(int)" />
    public void GLEnableVertexAttribArray( int index )
    {
        GL.glEnableVertexAttribArray( index );
    }

    //    public void GLFramebufferRenderbuffer( int target, int attachment, int int, int renderbuffer )
//    {
//        EXTFramebufferObject.glFramebufferRenderbufferEXT( target, attachment, int, renderbuffer );
//    }

//    public void GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level )
//    {
//        EXTFramebufferObject.glFramebufferTexture2DEXT( target, attachment, textarget, texture, level );
//    }

    /// <inheritdoc cref="GL.glFrontFace(int)" />
    public void GLFrontFace( int mode )
    {
        GL.glFrontFace( mode );
    }

    /// <inheritdoc cref="GL.glGenBuffers(int[])" />
    public void GLGenBuffers( int[] buffers )
    {
        GL.glGenBuffers( buffers );
    }

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

    /// <inheritdoc cref="GL.glGenTextures(int[])" />
    public void GLGenTextures( int[] textures )
    {
        GL.glGenTextures( textures );
    }

    //    public void GLGenerateMipmap( int target )
//    {
//        EXTFramebufferObject.glGenerateMipmapEXT( target );
//    }

    /// <inheritdoc cref="GL.glGetActiveAttrib(int,int,int,out int,out int,out int,StringBuilder)" />
    public void GLGetActiveAttrib( int program,
                                   int index,
                                   int bufSize,
                                   out int length,
                                   out int size,
                                   out int type,
                                   StringBuilder name )
    {
        GL.glGetActiveAttrib( program, index, bufSize, out length, out size, out type, name );
    }

    /// <inheritdoc cref="GL.glGetActiveUniform(int,int,int,out int, out int,out int,StringBuilder)" />
    public void GLGetActiveUniform( int program,
                                    int index,
                                    int bufSize,
                                    out int length,
                                    out int size,
                                    out int type,
                                    StringBuilder name )
    {
        GL.glGetActiveUniform( program, index, bufSize, out length, out size, out type, name );
    }

    /// <inheritdoc cref="GL.glGetAttachedShaders(int,out int,int[])" />
    public void GLGetAttachedShaders( int program, out int buffer, int[] shaders )
    {
        GL.glGetAttachedShaders( program, out buffer, shaders );
    }

    public void GLGetFloatv<T>( int pname, out T parameters ) where T : struct
    {
        GL.glGetFloat( pname, out parameters );
    }

    //    public void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters )
//    {
//        EXTFramebufferObject.glGetFramebufferAttachmentParameterivEXT( target, attachment, pname, parameters );
//    }

//    public void GLGetIntegerv( int pname, IntBuffer parameters )
//    {
//        GL.glGetIntegerv( pname, parameters );
//    }

//    public string GLGetProgramInfoLog( int program )
//    {
//        ByteBuffer buffer = ByteBuffer.AllocateDirect( 1024 * 10 );

//        buffer.Order( ByteOrder.NativeOrder );

//        ByteBuffer tmp = ByteBuffer.AllocateDirect( 4 );
//        tmp.Order( ByteOrder.NativeOrder );

//        IntBuffer intBuffer = tmp.AsIntBuffer();

//        GL.glGetProgramInfoLog( program, intBuffer, buffer );

//        var numBytes = intBuffer.Get( 0 );
//        var bytes    = new byte[ numBytes ];

//        buffer.Get( bytes );

//        return new string( bytes );
//    }

//    public void GLGetProgramiv( int program, int pname, IntBuffer parameters )
//    {
//        GL.glGetProgramiv( program, pname, parameters );
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

//        GL.glGetShaderInfoLog( shader, intBuffer, buffer );
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
//        GL.glGetShaderiv( shader, pname, parameters );
//    }

    /// <inheritdoc cref="GL.glGetString(int)" />
    public string GLGetString( int name )
    {
        return GL.glGetString( name );
    }

    //    public void GLGetUniformfv( int program, int location, FloatBuffer parameters )
//    {
//        GL.glGetUniformfv( program, location, parameters );
//    }

//    public void GLGetUniformiv( int program, int location, IntBuffer parameters )
//    {
//        GL.glGetUniformiv( program, location, parameters );
//    }

//    public void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer )
//    {
//        throw new UnsupportedOperationException( "unsupported, won't implement" );
//    }

//    public void GLGetVertexAttribfv( int index, int pname, FloatBuffer parameters )
//    {
//        GL.glGetVertexAttribfv( index, pname, parameters );
//    }

//    public void GLGetVertexAttribiv( int index, int pname, IntBuffer parameters )
//    {
//        GL.glGetVertexAttribiv( index, pname, parameters );
//    }

    /// <inheritdoc cref="GL.glHint(int,int)" />
    public void GLHint( int target, int mode )
    {
        GL.glHint( target, mode );
    }

    /// <inheritdoc cref="GL.glIsBuffer(int)" />
    public bool GLIsBuffer( int buffer )
    {
        return GL.glIsBuffer( buffer );
    }

    /// <inheritdoc cref="GL.glIsEnabled(int)" />
    public bool GLIsEnabled( int cap )
    {
        return GL.glIsEnabled( cap );
    }

    //    public bool GLIsFramebuffer( int framebuffer )
//    {
//        return EXTFramebufferObject.glIsFramebufferEXT( framebuffer );
//    }

    /// <inheritdoc cref="GL.glIsProgram(int)" />
    public bool GLIsProgram( int program )
    {
        return GL.glIsProgram( program );
    }

    //    public bool GLIsRenderbuffer( int renderbuffer )
//    {
//        return EXTFramebufferObject.glIsRenderbufferEXT( renderbuffer );
//    }

    /// <inheritdoc cref="GL.glIsShader(int)" />
    public bool GLIsShader( int shader )
    {
        return GL.glIsShader( shader );
    }

    /// <inheritdoc cref="GL.glIsTexture(int)" />
    public bool GLIsTexture( int texture )
    {
        return GL.glIsTexture( texture );
    }

    public void GLPixelStorei( int pname, int param )
    {
        GL.glPixelStore( pname, param );
    }

    /// <inheritdoc cref="GL.glReadPixels(int,int,int,int,int,int,IntPtr)" />
    public void GLReadPixels( int x, int y, int width, int height, int format, int type, IntPtr pixels )
    {
        GL.glReadPixels( x, y, width, height, format, type, pixels );
    }

    //    public void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length )
//    {
//        throw new UnsupportedOperationException( "unsupported, won't implement" );
//    }

    /// <inheritdoc cref="GL.glShaderSource(int,string[])" />
    public void GLShaderSource( int shader, string[] str )
    {
        GL.glShaderSource( shader, str );
    }

    /// <inheritdoc cref="GL.glStencilFunc(int,int,int)" />
    public void GLStencilFunc( int func, int reference, int mask )
    {
        GL.glStencilFunc( func, reference, mask );
    }

    /// <inheritdoc cref="GL.glStencilFuncSeparate(int,int,int,int)" />
    public void GLStencilFuncSeparate( int face, int func, int reference, int mask )
    {
        GL.glStencilFuncSeparate( face, func, reference, mask );
    }

    /// <inheritdoc cref="GL.glStencilMask(int)" />
    public void GLStencilMask( int mask )
    {
        GL.glStencilMask( mask );
    }

    /// <inheritdoc cref="GL.glStencilMaskSeparate(int,int)" />
    public void GLStencilMaskSeparate( int face, int mask )
    {
        GL.glStencilMaskSeparate( face, mask );
    }

    /// <inheritdoc cref="GL.glint(int,int,int)" />
    public void GLint( int fail, int zfail, int zpass )
    {
        GL.gl;
    }

    int( fail, zfail, zpass );

    /// <inheritdoc cref="GL.glintSeparate(int,int,int,int)" />
    public void GLintSeparate( int face, int fail, int zfail, int zpass )
    {
        GL.glintSeparate( face, fail, zfail, zpass );
    }

    public void GLTexImage2D( int target,
                              int level,
                              int int,
                              int width,
                              int height,
                              int border,
                              int format,
                              int type,
                              IntPtr pixels ) => GL.glTexImage2D( target, level, int, width, height, border, format, type, pixels );

    /// <inheritdoc cref="GL.glTexParameterf{T}(int,int,T)" />
    public void GLTexParameterf<T>( int target, int pname, T param ) where T : struct
    {
        GL.glTexParameterf( target, pname, param );
    }

    //    public void GLTexParameterfv( int target, int pname, FloatBuffer parameters )
//    {
//        GL.glTexParameterfv( target, pname, parameters );
//    }

    /// <inheritdoc cref="GL.glTexParameteri{T}(int,int,T)" />
    public void GLTexParameteri<T>( int target, int pname, T param ) where T : struct
    {
        GL.glTexParameteri( target, pname, param );
    }

    //    public void GLTexParameteriv( int target, int pname, IntBuffer parameters )
//    {
//        GL.glTexParameteriv( target, pname, parameters );
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
                GL.glTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, buffer );

                break;

            case ShortBuffer buffer:
                GL.glTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, buffer );

                break;

            case IntBuffer buffer:
                GL.glTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, buffer );

                break;

            case FloatBuffer buffer:
                GL.glTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, buffer );

                break;

            case DoubleBuffer buffer:
                GL.glTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, buffer );

                break;

            default:
                throw new GdxRuntimeException( $"Can't use {pixels.GetType().Name} with this method. "
                                             + $"Use ByteBuffer, ShortBuffer, IntBuffer, FloatBuffer "
                                             + $"or DoubleBuffer instead." );
        }
    }

    /// <inheritdoc cref="GL.glUniform1f{T}(int,int,T)" />
    public void GLUniform1F<T>( int location, int count, T value ) where T : struct
    {
        GL.glUniform1f( location, count, value );
    }

    //    public void GLUniform1Fv( int location, int count, FloatBuffer v )
//    {
//        GL.glUniform1fv( location, v );
//    }

//    public void GLUniform1Fv( int location, int count, float[] v, int offset )
//    {
//        GL.glUniform1fv( location, toFloatBuffer( v, offset, count ) );
//    }

    /// <inheritdoc cref="GL.glUniform1i{T}(int,int,T)" />
    public void GLUniform1I<T>( int location, int count, T value ) where T : struct
    {
        GL.glUniform1i( location, count, value );
    }

    //    public void GLUniform1Iv( int location, int count, IntBuffer v )
//    {
//        GL.glUniform1iv( location, v );
//    }

//    public void GLUniform1Iv( int location, int count, int[] v, int offset )
//    {
//        GL.glUniform1iv( location, toIntBuffer( v, offset, count ) );
//    }

    /// <inheritdoc cref="GL.glUniform2f{T}(int,int,T)" />
    public void GLUniform2F<T>( int location, int count, T value ) where T : struct
    {
        GL.glUniform2f( location, count, value );
    }

    //    public void GLUniform2Fv( int location, int count, FloatBuffer v )
//    {
//        GL.glUniform2fv( location, v );
//    }

//    public void GLUniform2Fv( int location, int count, float[] v, int offset )
//    {
//        GL.glUniform2fv( location, toFloatBuffer( v, offset, count << 1 ) );
//    }

    /// <inheritdoc cref="GL.glUniform2i{T}(int,int,T)" />
    public void GLUniform2I<T>( int location, int count, T value ) where T : struct
    {
        GL.glUniform2i( location, count, value );
    }

    //    public void GLUniform2Iv( int location, int count, IntBuffer v )
//    {
//        GL.glUniform2iv( location, v );
//    }

//    public void GLUniform2Iv( int location, int count, int[] v, int offset )
//    {
//        GL.glUniform2iv( location, toIntBuffer( v, offset, count << 1 ) );
//    }

    /// <inheritdoc cref="GL.glUniform3f{T}(int,int,T)" />
    public void GLUniform3F<T>( int location, int count, T value ) where T : struct
    {
        GL.glUniform3f( location, count, value );
    }

    //    public void GLUniform3Fv( int location, int count, FloatBuffer v )
//    {
//        GL.glUniform3fv( location, v );
//    }

//    public void GLUniform3Fv( int location, int count, float[] v, int offset )
//    {
//        GL.glUniform3fv( location, toFloatBuffer( v, offset, count * 3 ) );
//    }

    /// <inheritdoc cref="GL.glUniform3i{T}(int,int,T)" />
    public void GLUniform3I<T>( int location, int count, T value ) where T : struct
    {
        GL.glUniform3i( location, count, value );
    }

    //    public void GLUniform3Iv( int location, int count, IntBuffer v )
//    {
//        GL.glUniform3iv( location, v );
//    }

//    public void GLUniform3Iv( int location, int count, int[] v, int offset )
//    {
//        GL.glUniform3iv( location, ToIntBuffer( v, offset, count * 3 ) );
//    }

    /// <summary>
    ///     Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">
    ///     Specifies the location of the uniform variable to be modified.
    /// </param>
    /// <param name="count">
    ///     For the vector (GL.glUniform*v) commands, specifies the number of elements that are to be
    ///     modified. This should be 1 if the targeted uniform variable is not an array, and 1 or
    ///     more if it is an array.
    /// </param>
    /// <param name="value">
    ///     For the vector and matrix commands, specifies a pointer to an array of <paramref name="count" />
    ///     values that will be used to update the specified uniform variable.
    /// </param>
    public void GLUniform4F<T>( int location, int count, T value ) where T : struct
    {
        GL.glUniform4f( location, count, value );
    }

    //    public void GLUniform4Fv( int location, int count, FloatBuffer v )
//    {
//        GL.glUniform4fv( location, v );
//    }

//    public void GLUniform4Fv( int location, int count, float[] v, int offset )
//    {
//        GL.glUniform4fv( location, ToFloatBuffer( v, offset, count << 2 ) );
//    }

    /// <inheritdoc cref="GL.glUniform4i{T}(int,int,T)" />
    public void GLUniform4I<T>( int location, int count, T value ) where T : struct
    {
        GL.glUniform4i( location, count, value );
    }

    /// <inheritdoc cref="GL.glValidateProgram(int)" />
    public void GLValidateProgram( int program )
    {
        GL.glValidateProgram( program );
    }

    /// <inheritdoc cref="GL.glVertexAttrib1f{T}(int,T)" />
    public void GLVertexAttrib1F<T>( int indx, T x ) where T : struct
    {
        GL.glVertexAttrib1f( indx, x );
    }

    /// <inheritdoc cref="GL.glVertexAttrib1f{T}(int,T)" />
    public void GLVertexAttrib1Fv<T>( int indx, T values ) where T : struct
    {
        GL.glVertexAttrib1f( indx, values );
    }

    //    public void GLVertexAttrib2F( int indx, float x, float y )
//    {
//        //TODO:
//        throw new NotImplementedException();
//    }

    /// <inheritdoc cref="GL.glVertexAttrib2f{T}(int,T)" />
    public void GLVertexAttrib2Fv<T>( int indx, T values ) where T : struct
    {
        GL.glVertexAttrib2f( indx, values );
    }

    //    public void GLVertexAttrib3F( int indx, float x, float y, float z )
//    {
//        //TODO:
//        throw new NotImplementedException();
//    }

    /// <inheritdoc cref="GL.glVertexAttrib3f{T}(int,T)" />
    public void GLVertexAttrib3Fv<T>( int indx, T values ) where T : struct
    {
        GL.glVertexAttrib3f( indx, values );
    }

    //    public void GLVertexAttrib4F( int indx, float x, float y, float z, float w )
//    {
//        //TODO:
//        throw new NotImplementedException();
//    }

    /// <inheritdoc cref="GL.glVertexAttrib4f{T}(int,T)" />
    public void GLVertexAttrib4Fv<T>( int indx, T values ) where T : struct
    {
        GL.glVertexAttrib4f( indx, values );
    }

    /// <summary>
    ///     define an array of generic vertex attribute data.
    /// </summary>
    /// <param name="indx">
    ///     Specifies the index of the generic vertex attribute to be modified.
    /// </param>
    /// <param name="size">
    ///     Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4.
    ///     Additionally, the symbolic constant GL.glBGRA is accepted by GL.glVertexAttribPointer.
    ///     The initial value is 4.
    /// </param>
    /// <param name="type">
    ///     Specifies the data type of each component in the array. The symbolic constants GL.glBYTE,
    ///     GL.glUNSIGNED_BYTE, GL.glSHORT, GL.glUNSIGNED_SHORT, GL.glINT, and GL.glUNSIGNED_INT are accepted
    ///     by GL.glVertexAttribPointer and GL.glVertexAttribIPointer. Additionally GL.glHALF_FLOAT, GL.glFLOAT,
    ///     GL.glDOUBLE, GL.glFIXED, GL.glINT_2_10_10_10_REV, GL.glUNSIGNED_INT_2_10_10_10_REV and
    ///     GL.glUNSIGNED_INT_10F_11F_11F_REV are accepted by GL.glVertexAttribPointer. GL.glDOUBLE is also
    ///     accepted by GL.glVertexAttribLPointer and is the only token accepted by the type parameter
    ///     for that function. The initial value is GL.glFLOAT.
    /// </param>
    /// <param name="normalized">
    ///     For GL.glVertexAttribPointer, specifies whether fixed-point data values should be normalized
    ///     (GL.glTRUE) or converted directly as fixed-point values (GL.glFALSE) when they are accessed.
    /// </param>
    /// <param name="stride">
    ///     Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the
    ///     generic vertex attributes are understood to be tightly packed in the array.
    ///     The initial value is 0.
    /// </param>
    /// <param name="pointer">
    ///     Specifies a offset of the first component of the first generic vertex attribute in the array
    ///     in the data store of the buffer currently bound to the GL.glARRAY_BUFFER target.
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
                    GL.glVertexAttribPointer( indx, size, type, normalized, stride, byteBuffer );

                    break;

                case int.Short:
                case int.UnsignedShort:
                    GL.glVertexAttribPointer( indx, size, type, normalized, stride, byteBuffer.AsShortBuffer() );

                    break;

                case int.Float:
                    GL.glVertexAttribPointer( indx, size, type, normalized, stride, byteBuffer.AsFloatBuffer() );

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
                GL.glVertexAttribPointer( indx, size, type, normalized, stride, fBuffer );
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

    /// <inheritdoc cref="GL.glDrawElements(int,int,int,IntPtr)" />
    public void GLDrawElements( int mode, int count, int type, IntPtr indices )
    {
        GL.glDrawElements( mode, count, type, indices );
    }

    /// <inheritdoc cref="GL.glVertexAttribPointer(int,int,int,bool,int,IntPtr)" />
    public void GLVertexAttribPointer( int indx,
                                       int size,
                                       int type,
                                       bool normalized,
                                       int stride,
                                       IntPtr ptr )
    {
        GL.glVertexAttribPointer( indx, size, type, normalized, stride, ptr );
    }
}
