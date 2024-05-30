// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using System.Numerics;
using Buffer = LughSharp.LibCore.Utils.Buffers.Buffer;

namespace LughSharp.LibCore.Graphics.Profiling;

/// <summary>
/// A collection of OpenGL Profiling methods.
/// </summary>
[PublicAPI]
public class GLInterceptor : BaseGLInterceptor, IGLBindings
{
    public GLInterceptor( GLProfiler profiler )
        : base( profiler )
    {
    }

    /// <inheritdoc />
    public (int major, int minor) GetProjectOpenGLVersion()
    {
        Calls++;
        var (major, minor) = Gdx.GL.GetProjectOpenGLVersion();
        CheckErrors();

        return ( major, minor );
    }

    /// <inheritdoc />
    public int GetProjectOpenGLVersionMajor()
    {
        Calls++;
        var major = Gdx.GL.GetProjectOpenGLVersionMajor();
        CheckErrors();

        return major;
    }

    /// <inheritdoc />
    public int GetProjectOpenGLVersionMinor()
    {
        Calls++;
        var minor = Gdx.GL.GetProjectOpenGLVersionMinor();
        CheckErrors();

        return minor;
    }

    /// <inheritdoc />
    public string GetProjectOpenGLProfile()
    {
        Calls++;
        var profile = Gdx.GL.GetProjectOpenGLProfile();
        CheckErrors();

        return profile;
    }

    /// <inheritdoc />
    public void glCullFace( int mode )
    {
        Calls++;
        Gdx.GL.glCullFace( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glFrontFace( int mode )
    {
        Calls++;
        Gdx.GL.glFrontFace( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glHint( int target, int mode )
    {
        Calls++;
        Gdx.GL.glHint( target, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glLineWidth( float width )
    {
        Calls++;
        Gdx.GL.glLineWidth( width );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glPointSize( float size )
    {
        Calls++;
        Gdx.GL.glPointSize( size );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glPolygonMode( int face, int mode )
    {
        Calls++;
        Gdx.GL.glPolygonMode( face, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glScissor( int x, int y, int width, int height )
    {
        Calls++;
        Gdx.GL.glScissor( x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glTexParameterf( int target, int pname, float param )
    {
        Calls++;
        Gdx.GL.glTexParameterf( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexParameterfv( int target, int pname, float* @params )
    {
        Calls++;
        Gdx.GL.glTexParameterfv( target, pname, @params );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glTexParameterfv( int target, int pname, float[] @params )
    {
        Calls++;
        Gdx.GL.glTexParameterfv( target, pname, @params );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glTexParameteri( int target, int pname, int param )
    {
        Calls++;
        Gdx.GL.glTexParameteri( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexParameteriv( int target, int pname, int* @params )
    {
        Calls++;
        Gdx.GL.glTexParameteriv( target, pname, @params );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexParameteriv( int target, int pname, int[] @params )
    {
        Calls++;
        Gdx.GL.glTexParameteriv( target, pname, @params );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexImage1D( int target, int level, int internalformat, int width, int border, int format, int type, void* pixels )
    {
        Calls++;
        Gdx.GL.glTexImage1D( target, level, internalformat, width, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexImage1D< T >( int target, int level, int internalformat, int width, int border, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Gdx.GL.glTexImage1D( target, level, internalformat, width, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexImage2D( int target, int level, int internalformat, int width, int height, int border, int format, int type, void* pixels )
    {
        Calls++;
        Gdx.GL.glTexImage2D( target, level, internalformat, width, height, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexImage2D< T >( int target, int level, int internalformat, int width, int height, int border, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Gdx.GL.glTexImage2D( target, level, internalformat, width, height, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glDrawBuffer( int buf )
    {
        Calls++;
        Gdx.GL.glDrawBuffer( buf );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glClear( uint mask )
    {
        Calls++;
        Gdx.GL.glClear( mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glClearColor( float red, float green, float blue, float alpha )
    {
        Calls++;
        Gdx.GL.glClearColor( red, green, blue, alpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glClearStencil( int s )
    {
        Calls++;
        Gdx.GL.glClearStencil( s );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glClearDepth( double depth )
    {
        Calls++;
        Gdx.GL.glClearDepth( depth );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glStencilMask( uint mask )
    {
        Calls++;
        Gdx.GL.glStencilMask( mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glColorMask( bool red, bool green, bool blue, bool alpha )
    {
        Calls++;
        Gdx.GL.glColorMask( red, green, blue, alpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glDepthMask( bool flag )
    {
        Calls++;
        Gdx.GL.glDepthMask( flag );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glDisable( int cap )
    {
        Calls++;
        Gdx.GL.glDisable( cap );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glEnable( int cap )
    {
        Calls++;
        Gdx.GL.glEnable( cap );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glFinish()
    {
        Calls++;
        Gdx.GL.glFinish();
        CheckErrors();
    }

    /// <inheritdoc />
    public void glFlush()
    {
        Calls++;
        Gdx.GL.glFlush();
        CheckErrors();
    }

    /// <inheritdoc />
    public void glBlendFunc( int sfactor, int dfactor )
    {
        Calls++;
        Gdx.GL.glBlendFunc( sfactor, dfactor );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glLogicOp( int opcode )
    {
        Calls++;
        Gdx.GL.glLogicOp( opcode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glStencilFunc( int func, int @ref, uint mask )
    {
        Calls++;
        Gdx.GL.glStencilFunc( func, @ref, mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glStencilOp( int fail, int zfail, int zpass )
    {
        Calls++;
        Gdx.GL.glStencilOp( fail, zfail, zpass );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glDepthFunc( int func )
    {
        Calls++;
        Gdx.GL.glDepthFunc( func );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glPixelStoref( int pname, float param )
    {
        Calls++;
        Gdx.GL.glPixelStoref( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glPixelStorei( int pname, int param )
    {
        Calls++;
        Gdx.GL.glPixelStorei( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void glReadBuffer( int src )
    {
        Calls++;
        Gdx.GL.glReadBuffer( src );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glReadPixels( int x, int y, int width, int height, int format, int type, void* pixels )
    {
        Calls++;
        Gdx.GL.glReadPixels( x, y, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glReadPixels< T >( int x, int y, int width, int height, int format, int type, ref T[] pixels ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.glReadPixels( x, y, width, height, format, type, ref pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetBooleanv( int pname, bool* data )
    {
        Calls++;
        Gdx.GL.glGetBooleanv( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetBooleanv( int pname, ref bool[] data )
    {
        Calls++;
        Gdx.GL.glGetBooleanv( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetDoublev( int pname, double* data )
    {
        Calls++;
        Gdx.GL.glGetDoublev( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetDoublev( int pname, ref double[] data )
    {
        Calls++;
        Gdx.GL.glGetDoublev( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public int glGetError()
    {
        Calls++;
        var err = Gdx.GL.glGetError();
        CheckErrors();

        return err;
    }

    /// <inheritdoc />
    public void glGetFloatv( int pname, float[] data )
    {
        Calls++;
        Gdx.GL.glGetFloatv( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetFloatv( int pname, ref float[] data )
    {
        Calls++;
        Gdx.GL.glGetFloatv( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetIntegerv( int pname, int* data )
    {
        Calls++;
        Gdx.GL.glGetIntegerv( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetIntegerv( int pname, ref int[] data )
    {
        Calls++;
        Gdx.GL.glGetIntegerv( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe byte* glGetString( int name )
    {
        Calls++;
        var str = Gdx.GL.glGetString( name );
        CheckErrors();

        return str;
    }

    /// <inheritdoc />
    public unsafe string glGetStringSafe( int name )
    {
        Calls++;
        var str = Gdx.GL.glGetStringSafe( name ); 
        CheckErrors();

        return str;
    }

    /// <inheritdoc />
    public unsafe void glGetTexImage( int target, int level, int format, int type, void* pixels )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTexImage< T >( int target, int level, int format, int type, ref T[] pixels ) where T : unmanaged
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTexParameterfv( int target, int pname, float* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTexParameterfv( int target, int pname, ref float[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTexParameteriv( int target, int pname, int* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTexParameteriv( int target, int pname, ref int[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTexLevelParameterfv( int target, int level, int pname, float* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTexLevelParameterfv( int target, int level, int pname, ref float[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTexLevelParameteriv( int target, int level, int pname, int* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTexLevelParameteriv( int target, int level, int pname, ref int[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public bool glIsEnabled( int cap )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public void glDepthRange( double near, double far )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glViewport( int x, int y, int width, int height )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glDrawArrays( int mode, int first, int count )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDrawElements( int mode, int count, int type, void* indices )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDrawElements< T >( int mode, int count, int type, T[] indices ) where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glPolygonOffset( float factor, float units )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glCopyTexImage1D( int target, int level, int internalformat, int x, int y, int width, int border )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glCopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height, int border )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glCopyTexSubImage1D( int target, int level, int xoffset, int x, int y, int width )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glCopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexSubImage1D( int target, int level, int xoffset, int width, int format, int type, void* pixels )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexSubImage1D< T >( int target, int level, int xoffset, int width, int format, int type, T[] pixels ) where T : unmanaged
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, void* pixels )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexSubImage2D< T >( int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glBindTexture( int target, uint texture )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDeleteTextures( int n, uint* textures )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDeleteTextures( params uint[] textures )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGenTextures( int n, uint* textures )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint[] glGenTextures( int n )
    {
        Calls++;
        CheckErrors();

        return null!;
    }

    /// <inheritdoc />
    public uint glGenTexture()
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public bool glIsTexture( uint texture )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public unsafe void glDrawRangeElements( int mode, uint start, uint end, int count, int type, void* indices )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDrawRangeElements< T >( int mode, uint start, uint end, int count, int type, T[] indices ) where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexImage3D( int target,
                                     int level,
                                     int internalformat,
                                     int width,
                                     int height,
                                     int depth,
                                     int border,
                                     int format,
                                     int type,
                                     void* pixels )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexImage3D< T >( int target,
                                          int level,
                                          int internalformat,
                                          int width,
                                          int height,
                                          int depth,
                                          int border,
                                          int format,
                                          int type,
                                          T[] pixels ) where T : unmanaged
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexSubImage3D( int target,
                                        int level,
                                        int xoffset,
                                        int yoffset,
                                        int zoffset,
                                        int width,
                                        int height,
                                        int depth,
                                        int format,
                                        int type,
                                        void* pixels )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexSubImage3D< T >( int target,
                                             int level,
                                             int xoffset,
                                             int yoffset,
                                             int zoffset,
                                             int width,
                                             int height,
                                             int depth,
                                             int format,
                                             int type,
                                             T[] pixels ) where T : unmanaged
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glCopyTexSubImage3D( int target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glActiveTexture( int texture )
    {
    }

    /// <inheritdoc />
    public void glSampleCoverage( float value, bool invert )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glCompressedTexImage3D( int target,
                                               int level,
                                               int internalformat,
                                               int width,
                                               int height,
                                               int depth,
                                               int border,
                                               int imageSize,
                                               void* data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glCompressedTexImage3D( int target, int level, int internalformat, int width, int height, int depth, int border, byte[] data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glCompressedTexImage2D( int target, int level, int internalformat, int width, int height, int border, int imageSize, void* data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glCompressedTexImage2D( int target, int level, int internalformat, int width, int height, int border, byte[] data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glCompressedTexImage1D( int target, int level, int internalformat, int width, int border, int imageSize, void* data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glCompressedTexImage1D( int target, int level, int internalformat, int width, int border, byte[] data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glCompressedTexSubImage3D( int target,
                                                  int level,
                                                  int xoffset,
                                                  int yoffset,
                                                  int zoffset,
                                                  int width,
                                                  int height,
                                                  int depth,
                                                  int format,
                                                  int imageSize,
                                                  void* data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glCompressedTexSubImage3D( int target,
                                                  int level,
                                                  int xoffset,
                                                  int yoffset,
                                                  int zoffset,
                                                  int width,
                                                  int height,
                                                  int depth,
                                                  int format,
                                                  byte[] data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glCompressedTexSubImage2D( int target,
                                                  int level,
                                                  int xoffset,
                                                  int yoffset,
                                                  int width,
                                                  int height,
                                                  int format,
                                                  int imageSize,
                                                  void* data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glCompressedTexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height, int format, byte[] data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glCompressedTexSubImage1D( int target, int level, int xoffset, int width, int format, int imageSize, void* data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glCompressedTexSubImage1D( int target, int level, int xoffset, int width, int format, byte[] data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetCompressedTexImage( int target, int level, void* img )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetCompressedTexImage( int target, int level, ref byte[] img )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glBlendFuncSeparate( int sfactorRGB, int dfactorRGB, int sfactorAlpha, int dfactorAlpha )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glMultiDrawArrays( int mode, int* first, int* count, int drawcount )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glMultiDrawArrays( int mode, int[] first, int[] count )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glMultiDrawElements( int mode, int* count, int type, void** indices, int drawcount )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glMultiDrawElements< T >( int mode, int[] count, int type, T[][] indices ) where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glPointParameterf( int pname, float param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glPointParameterfv( int pname, float* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glPointParameterfv( int pname, float[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glPointParameteri( int pname, int param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glPointParameteriv( int pname, int* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glPointParameteriv( int pname, int[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glBlendColor( float red, float green, float blue, float alpha )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glBlendEquation( int mode )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGenQueries( int n, uint* ids )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint[] glGenQueries( int n )
    {
        Calls++;
        CheckErrors();

        return null!;
    }

    /// <inheritdoc />
    public uint glGenQuery()
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe void glDeleteQueries( int n, uint* ids )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDeleteQueries( params uint[] ids )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public bool glIsQuery( uint id )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public void glBeginQuery( int target, uint id )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glEndQuery( int target )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetQueryiv( int target, int pname, int* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetQueryiv( int target, int pname, ref int[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetQueryObjectiv( uint id, int pname, int* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetQueryObjectiv( uint id, int pname, ref int[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetQueryObjectuiv( uint id, int pname, uint* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetQueryObjectuiv( uint id, int pname, ref uint[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glBindBuffer( int target, uint buffer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDeleteBuffers( int n, uint* buffers )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDeleteBuffers( params uint[] buffers )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGenBuffers( int n, uint* buffers )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint[] glGenBuffers( int n )
    {
        Calls++;
        CheckErrors();

        return null!;
    }

    /// <inheritdoc />
    public uint glGenBuffer()
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public bool glIsBuffer( uint buffer )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public unsafe void glBufferData( int target, int size, void* data, int usage )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glBufferData< T >( int target, T[] data, int usage ) where T : unmanaged
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glBufferSubData( int target, int offset, int size, void* data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glBufferSubData< T >( int target, int offsetCount, T[] data ) where T : unmanaged
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetBufferSubData( int target, int offset, int size, void* data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetBufferSubData< T >( int target, int offsetCount, int count, ref T[] data ) where T : unmanaged
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* glMapBuffer( int target, int access )
    {
        Calls++;
        CheckErrors();

        return null;
    }

    /// <inheritdoc />
    public unsafe Span< T > glMapBuffer< T >( int target, int access ) where T : unmanaged
    {
        Calls++;
        CheckErrors();

        return null!;
    }

    /// <inheritdoc />
    public bool glUnmapBuffer( int target )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public unsafe void glGetBufferParameteriv( int target, int pname, int* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetBufferParameteriv( int target, int pname, ref int[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetBufferPointerv( int target, int pname, void** @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetBufferPointerv( int target, int pname, ref IntPtr[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glBlendEquationSeparate( int modeRGB, int modeAlpha )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDrawBuffers( int n, int* bufs )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDrawBuffers( params int[] bufs )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glStencilOpSeparate( int face, int sfail, int dpfail, int dppass )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glStencilFuncSeparate( int face, int func, int @ref, uint mask )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glStencilMaskSeparate( int face, uint mask )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glAttachShader( uint program, uint shader )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glBindAttribLocation( uint program, uint index, byte* name )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glBindAttribLocation( uint program, uint index, string name )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glCompileShader( uint shader )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public uint glCreateProgram()
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public uint glCreateShader( int type )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public void glDeleteProgram( uint program )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glDeleteShader( uint shader )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glDetachShader( uint program, uint shader )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glDisableVertexAttribArray( uint index )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glEnableVertexAttribArray( uint index )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetActiveAttrib( uint program, uint index, int bufSize, int* length, int* size, int* type, byte* name )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe string glGetActiveAttrib( uint program, uint index, int bufSize, out int size, out int type )
    {
        Calls++;
        CheckErrors();
        size = 0;
        type = 0;

        return null;
    }

    /// <inheritdoc />
    public unsafe void glGetActiveUniform( uint program, uint index, int bufSize, int* length, int* size, int* type, byte* name )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe string glGetActiveUniform( uint program, uint index, int bufSize, out int size, out int type )
    {
        Calls++;
        CheckErrors();
        size = 0;
        type = 0;

        return null;
    }

    /// <inheritdoc />
    public string glGetActiveUniform( int handle, int u, IntBuffer bufSize, Buffer buffer )
    {
        Calls++;
        CheckErrors();

        return null;
    }

    /// <inheritdoc />
    public unsafe void glGetAttachedShaders( uint program, int maxCount, int* count, uint* shaders )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint[] glGetAttachedShaders( uint program, int maxCount )
    {
        Calls++;
        CheckErrors();

        return null!;
    }

    /// <inheritdoc />
    public unsafe int glGetAttribLocation( uint program, byte* name )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe int glGetAttribLocation( uint program, string name )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe void glGetProgramiv( uint program, int pname, int* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetProgramiv( uint program, int pname, ref int[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetProgramiv( uint program, int pname, Buffer buffer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetProgramInfoLog( uint program, int bufSize, int* length, byte* infoLog )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe string glGetProgramInfoLog( uint program, int bufSize )
    {
        Calls++;
        CheckErrors();

        return null;
    }

    /// <inheritdoc />
    public unsafe void glGetShaderiv( uint shader, int pname, int* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetShaderiv( uint shader, int pname, ref int[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetShaderInfoLog( uint shader, int bufSize, int* length, byte* infoLog )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe string glGetShaderInfoLog( uint shader, int bufSize )
    {
        Calls++;
        CheckErrors();

        return null;
    }

    /// <inheritdoc />
    public unsafe void glGetShaderSource( uint shader, int bufSize, int* length, byte* source )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe string glGetShaderSource( uint shader, int bufSize = 4096 )
    {
        Calls++;
        CheckErrors();

        return null;
    }

    /// <inheritdoc />
    public unsafe int glGetUniformLocation( uint program, byte* name )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe int glGetUniformLocation( uint program, string name )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe void glGetUniformfv( uint program, int location, float* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetUniformfv( uint program, int location, ref float[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetUniformiv( uint program, int location, int* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetUniformiv( uint program, int location, ref int[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetVertexAttribdv( uint index, int pname, double* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetVertexAttribdv( uint index, int pname, ref double[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetVertexAttribfv( uint index, int pname, float* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetVertexAttribfv( uint index, int pname, ref float[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetVertexAttribiv( uint index, int pname, int* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetVertexAttribiv( uint index, int pname, ref int[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetVertexAttribPointerv( uint index, int pname, void** pointer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetVertexAttribPointerv( uint index, int pname, ref uint[] pointer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public bool glIsProgram( uint program )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public bool glIsShader( uint shader )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public void glLinkProgram( uint program )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glShaderSource( uint shader, int count, byte** @string, int* length )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glShaderSource( uint shader, params string[] @string )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glUseProgram( uint program )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glUniform1f( int location, float v0 )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glUniform2f( int location, float v0, float v1 )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glUniform3f( int location, float v0, float v1, float v2 )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glUniform4f( int location, float v0, float v1, float v2, float v3 )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glUniform1i( int location, int v0 )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glUniform2i( int location, int v0, int v1 )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glUniform3i( int location, int v0, int v1, int v2 )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glUniform4i( int location, int v0, int v1, int v2, int v3 )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform1fv( int location, int count, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform1fv( int location, params float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform2fv( int location, int count, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform2fv( int location, params float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform3fv( int location, int count, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform3fv( int location, params float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform4fv( int location, int count, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform4fv( int location, params float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform1iv( int location, int count, int* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform1iv( int location, params int[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform2iv( int location, int count, int* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform2iv( int location, params int[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform3iv( int location, int count, int* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform3iv( int location, params int[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform4iv( int location, int count, int* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform4iv( int location, params int[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix2fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix2fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix3fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix3fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix4fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix4fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix4fv( int location, int count, bool transpose, Buffer buffer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public bool glValidateProgram( uint program )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public void glVertexAttrib1d( uint index, double x )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib1dv( uint index, double* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib1dv( uint index, params double[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttrib1f( uint index, float x )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib1fv( uint index, float* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib1fv( uint index, params float[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttrib1s( uint index, short x )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib1sv( uint index, short* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib1sv( uint index, params short[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttrib2d( uint index, double x, double y )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib2dv( uint index, double* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib2dv( uint index, params double[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttrib2f( uint index, float x, float y )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib2fv( uint index, float* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib2fv( uint index, params float[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttrib2s( uint index, short x, short y )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib2sv( uint index, short* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib2sv( uint index, params short[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttrib3d( uint index, double x, double y, double z )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib3dv( uint index, double* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib3dv( uint index, params double[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttrib3f( uint index, float x, float y, float z )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib3fv( uint index, float* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib3fv( uint index, params float[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttrib3s( uint index, short x, short y, short z )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib3sv( uint index, short* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib3sv( uint index, params short[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4Nbv( uint index, sbyte* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4Nbv( uint index, params sbyte[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4Niv( uint index, int* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4Niv( uint index, params int[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4Nsv( uint index, short* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4Nsv( uint index, params short[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttrib4Nub( uint index, byte x, byte y, byte z, byte w )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4Nubv( uint index, byte* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4Nubv( uint index, params byte[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4Nuiv( uint index, uint* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4Nuiv( uint index, params uint[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4Nusv( uint index, ushort* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4Nusv( uint index, params ushort[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4bv( uint index, sbyte* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4bv( uint index, params sbyte[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttrib4d( uint index, double x, double y, double z, double w )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4dv( uint index, double* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4dv( uint index, params double[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttrib4f( uint index, float x, float y, float z, float w )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4fv( uint index, float* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4fv( uint index, params float[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4iv( uint index, int* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4iv( uint index, params int[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttrib4s( uint index, short x, short y, short z, short w )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4sv( uint index, short* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4sv( uint index, params short[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4ubv( uint index, byte* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4ubv( uint index, params byte[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4uiv( uint index, uint* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4uiv( uint index, params uint[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4usv( uint index, ushort* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttrib4usv( uint index, params ushort[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribPointer( uint index, int size, int type, bool normalized, int stride, void* pointer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribPointer( uint index, int size, int type, bool normalized, int stride, uint pointer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribPointer( int location, int size, int type, bool normalized, int stride, Buffer buffer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix2x3fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix2x3fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix3x2fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix3x2fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix2x4fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix2x4fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix4x2fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix4x2fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix3x4fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix3x4fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix4x3fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniformMatrix4x3fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glColorMaski( uint index, bool r, bool g, bool b, bool a )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetBooleani_v( int target, uint index, bool* data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetBooleani_v( int target, uint index, ref bool[] data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetIntegeri_v( int target, uint index, int* data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetIntegeri_v( int target, uint index, ref int[] data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glEnablei( int target, uint index )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glDisablei( int target, uint index )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public bool glIsEnabledi( int target, uint index )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public void glBeginTransformFeedback( int primitiveMode )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glEndTransformFeedback()
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glBindBufferRange( int target, uint index, uint buffer, int offset, int size )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glBindBufferBase( int target, uint index, uint buffer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTransformFeedbackVaryings( uint program, int count, byte** varyings, int bufferMode )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTransformFeedbackVaryings( uint program, string[] varyings, int bufferMode )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTransformFeedbackVarying( uint program, uint index, int bufSize, int* length, int* size, int* type, byte* name )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe string glGetTransformFeedbackVarying( uint program, uint index, int bufSize, out int size, out int type )
    {
        Calls++;
        CheckErrors();
        size = 0;
        type = 0;

        return null;
    }

    /// <inheritdoc />
    public void glClampColor( int target, bool clamp )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glBeginConditionalRender( uint id, int mode )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glEndConditionalRender()
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribIPointer( uint index, int size, int type, int stride, void* pointer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribIPointer( uint index, int size, int type, int stride, uint pointer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetVertexAttribIiv( uint index, int pname, int* parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetVertexAttribIiv( uint index, int pname, ref int[] parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetVertexAttribIuiv( uint index, int pname, uint* parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetVertexAttribIuiv( uint index, int pname, ref uint[] parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttribI1i( uint index, int x )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttribI2i( uint index, int x, int y )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttribI3i( uint index, int x, int y, int z )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttribI4i( uint index, int x, int y, int z, int w )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttribI1ui( uint index, uint x )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttribI2ui( uint index, uint x, uint y )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttribI3ui( uint index, uint x, uint y, uint z )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttribI4ui( uint index, uint x, uint y, uint z, uint w )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI1iv( uint index, int* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI1iv( uint index, int[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI2iv( uint index, int* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI2iv( uint index, int[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI3iv( uint index, int* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI3iv( uint index, int[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI4iv( uint index, int* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI4iv( uint index, int[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI1uiv( uint index, uint* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI1uiv( uint index, uint[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI2uiv( uint index, uint* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI2uiv( uint index, uint[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI3uiv( uint index, uint* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI3uiv( uint index, uint[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI4uiv( uint index, uint* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI4uiv( uint index, uint[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI4bv( uint index, sbyte* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI4bv( uint index, sbyte[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI4sv( uint index, short* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI4sv( uint index, short[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI4ubv( uint index, byte* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI4ubv( uint index, byte[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI4usv( uint index, ushort* v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribI4usv( uint index, ushort[] v )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetUniformuiv( uint program, int location, uint* @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetUniformuiv( uint program, int location, ref uint[] @params )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glBindFragDataLocation( uint program, uint color, byte* name )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glBindFragDataLocation( uint program, uint color, string name )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int glGetFragDataLocation( uint program, byte* name )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe int glGetFragDataLocation( uint program, string name )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public void glUniform1ui( int location, uint v0 )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glUniform2ui( int location, uint v0, uint v1 )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glUniform3ui( int location, uint v0, uint v1, uint v2 )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glUniform4ui( int location, uint v0, uint v1, uint v2, uint v3 )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform1uiv( int location, int count, uint* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform1uiv( int location, uint[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform2uiv( int location, int count, uint* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform2uiv( int location, uint[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform3uiv( int location, int count, uint* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform3uiv( int location, uint[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform4uiv( int location, int count, uint* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glUniform4uiv( int location, uint[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexParameterIiv( int target, int pname, int* param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexParameterIiv( int target, int pname, int[] param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexParameterIuiv( int target, int pname, uint* param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glTexParameterIuiv( int target, int pname, uint[] param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTexParameterIiv( int target, int pname, int* parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTexParameterIiv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTexParameterIuiv( int target, int pname, uint* parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetTexParameterIuiv( int target, int pname, ref uint[] parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glClearBufferiv( int buffer, int drawbuffer, int* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glClearBufferiv( int buffer, int drawbuffer, int[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glClearBufferuiv( int buffer, int drawbuffer, uint* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glClearBufferuiv( int buffer, int drawbuffer, uint[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glClearBufferfv( int buffer, int drawbuffer, float* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glClearBufferfv( int buffer, int drawbuffer, float[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glClearBufferfi( int buffer, int drawbuffer, float depth, int stencil )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe byte* glGetStringi( int name, uint index )
    {
        Calls++;
        CheckErrors();

        return null;
    }

    /// <inheritdoc />
    public unsafe string glGetStringiSafe( int name, uint index )
    {
        Calls++;
        CheckErrors();

        return null;
    }

    /// <inheritdoc />
    public bool glIsRenderbuffer( uint renderbuffer )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public void glBindRenderbuffer( int target, uint renderbuffer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDeleteRenderbuffers( int n, uint* renderbuffers )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDeleteRenderbuffers( params uint[] renderbuffers )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGenRenderbuffers( int n, uint* renderbuffers )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint[] glGenRenderbuffers( int n )
    {
        Calls++;
        CheckErrors();

        return null!;
    }

    /// <inheritdoc />
    public uint glGenRenderbuffer()
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public void glRenderbufferStorage( int target, int internalformat, int width, int height )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetRenderbufferParameteriv( int target, int pname, int* parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetRenderbufferParameteriv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public bool glIsFramebuffer( uint framebuffer )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public void glBindFramebuffer( int target, uint framebuffer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDeleteFramebuffers( int n, uint* framebuffers )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDeleteFramebuffers( params uint[] framebuffers )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGenFramebuffers( int n, uint* framebuffers )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint[] glGenFramebuffers( int n )
    {
        Calls++;
        CheckErrors();

        return null!;
    }

    /// <inheritdoc />
    public uint glGenFramebuffer()
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public int glCheckFramebufferStatus( int target )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public void glFramebufferTexture1D( int target, int attachment, int textarget, uint texture, int level )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glFramebufferTexture2D( int target, int attachment, int textarget, uint texture, int level )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glFramebufferTexture3D( int target, int attachment, int textarget, uint texture, int level, int zoffset )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, uint renderbuffer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, int* parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, ref int[] parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glGenerateMipmap( int target )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glBlitFramebuffer( int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, int filter )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glRenderbufferStorageMultisample( int target, int samples, int internalformat, int width, int height )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glFramebufferTextureLayer( int target, int attachment, uint texture, int level, int layer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* glMapBufferRange( int target, int offset, int length, uint access )
    {
        Calls++;
        CheckErrors();

        return null;
    }

    /// <inheritdoc />
    public unsafe Span< T > glMapBufferRange< T >( int target, int offset, int length, uint access ) where T : unmanaged
    {
        Calls++;
        CheckErrors();

        return default;
    }

    /// <inheritdoc />
    public void glFlushMappedBufferRange( int target, int offset, int length )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glBindVertexArray( uint array )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDeleteVertexArrays( int n, uint* arrays )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDeleteVertexArrays( params uint[] arrays )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGenVertexArrays( int n, uint* arrays )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint[] glGenVertexArrays( int n )
    {
        Calls++;
        CheckErrors();

        return null!;
    }

    /// <inheritdoc />
    public unsafe uint glGenVertexArray()
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public bool glIsVertexArray( uint array )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public void glDrawArraysInstanced( int mode, int first, int count, int instancecount )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDrawElementsInstanced( int mode, int count, int type, void* indices, int instancecount )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDrawElementsInstanced< T >( int mode, int count, int type, T[] indices, int instancecount ) where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glTexBuffer( int target, int internalformat, uint buffer )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glPrimitiveRestartIndex( uint index )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glCopyBufferSubData( int readTarget, int writeTarget, int readOffset, int writeOffset, int size )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetUniformIndices( uint program, int uniformCount, byte** uniformNames, uint* uniformIndices )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint[] glGetUniformIndices( uint program, params string[] uniformNames )
    {
        Calls++;
        CheckErrors();

        return null!;
    }

    /// <inheritdoc />
    public unsafe void glGetActiveUniformsiv( uint program, int uniformCount, uint* uniformIndices, int pname, int* parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int[] glGetActiveUniformsiv( uint program, int pname, params uint[] uniformIndices )
    {
        Calls++;
        CheckErrors();

        return null!;
    }

    /// <inheritdoc />
    public unsafe void glGetActiveUniformName( uint program, uint uniformIndex, int bufSize, int* length, byte* uniformName )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe string glGetActiveUniformName( uint program, uint uniformIndex, int bufSize )
    {
        Calls++;
        CheckErrors();

        return null;
    }

    /// <inheritdoc />
    public unsafe uint glGetUniformBlockIndex( uint program, byte* uniformBlockName )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe uint glGetUniformBlockIndex( uint program, string uniformBlockName )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe void glGetActiveUniformBlockiv( uint program, uint uniformBlockIndex, int pname, int* parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetActiveUniformBlockiv( uint program, uint uniformBlockIndex, int pname, ref int[] parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetActiveUniformBlockName( uint program, uint uniformBlockIndex, int bufSize, int* length, byte* uniformBlockName )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe string glGetActiveUniformBlockName( uint program, uint uniformBlockIndex, int bufSize )
    {
        Calls++;
        CheckErrors();

        return null;
    }

    /// <inheritdoc />
    public void glUniformBlockBinding( uint program, uint uniformBlockIndex, uint uniformBlockBinding )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDrawElementsBaseVertex( int mode, int count, int type, void* indices, int basevertex )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDrawElementsBaseVertex< T >( int mode, int count, int type, T[] indices, int basevertex ) where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDrawRangeElementsBaseVertex( int mode, uint start, uint end, int count, int type, void* indices, int basevertex )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDrawRangeElementsBaseVertex< T >( int mode, uint start, uint end, int count, int type, T[] indices, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDrawElementsInstancedBaseVertex( int mode, int count, int type, void* indices, int instancecount, int basevertex )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDrawElementsInstancedBaseVertex< T >( int mode, int count, int type, T[] indices, int instancecount, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glMultiDrawElementsBaseVertex( int mode, int* count, int type, void** indices, int drawcount, int* basevertex )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glMultiDrawElementsBaseVertex< T >( int mode, int type, T[][] indices, int[] basevertex ) where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glProvokingVertex( int mode )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* glFenceSync( int condition, uint flags )
    {
        Calls++;
        CheckErrors();

        return null;
    }

    /// <inheritdoc />
    public unsafe IntPtr glFenceSyncSafe( int condition, uint flags )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe bool glIsSync( void* sync )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public unsafe bool glIsSyncSafe( IntPtr sync )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public unsafe void glDeleteSync( void* sync )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDeleteSyncSafe( IntPtr sync )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int glClientWaitSync( void* sync, uint flags, ulong timeout )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe int glClientWaitSyncSafe( IntPtr sync, uint flags, ulong timeout )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe void glWaitSync( void* sync, uint flags, ulong timeout )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glWaitSyncSafe( IntPtr sync, uint flags, ulong timeout )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetInteger64v( int pname, long* data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetInteger64v( int pname, ref long[] data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetSynciv( void* sync, int pname, int bufSize, int* length, int* values )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int[] glGetSynciv( IntPtr sync, int pname, int bufSize )
    {
        Calls++;
        CheckErrors();

        return null!;
    }

    /// <inheritdoc />
    public unsafe void glGetInteger64i_v( int target, uint index, long* data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetInteger64i_v( int target, uint index, ref long[] data )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetBufferParameteri64v( int target, int pname, long* parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetBufferParameteri64v( int target, int pname, ref long[] parameters )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glFramebufferTexture( int target, int attachment, uint texture, int level )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glTexImage2DMultisample( int target, int samples, int internalformat, int width, int height, bool fixedsamplelocations )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glTexImage3DMultisample( int target, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetMultisamplefv( int pname, uint index, float* val )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetMultisamplefvSafe( int pname, uint index, ref float[] val )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glSampleMaski( uint maskNumber, uint mask )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glBindFragDataLocationIndexed( uint program, uint colorNumber, uint index, byte* name )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glBindFragDataLocationIndexed( uint program, uint colorNumber, uint index, string name )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int glGetFragDataIndex( uint program, byte* name )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe int glGetFragDataIndex( uint program, string name )
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe void glGenSamplers( int count, uint* samplers )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint[] glGenSamplers( int count )
    {
        Calls++;
        CheckErrors();

        return null!;
    }

    /// <inheritdoc />
    public uint glGenSampler()
    {
        Calls++;
        CheckErrors();

        return 0;
    }

    /// <inheritdoc />
    public unsafe void glDeleteSamplers( int count, uint* samplers )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glDeleteSamplers( params uint[] samplers )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public bool glIsSampler( uint sampler )
    {
        Calls++;
        CheckErrors();

        return false;
    }

    /// <inheritdoc />
    public void glBindSampler( uint unit, uint sampler )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glSamplerParameteri( uint sampler, int pname, int param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glSamplerParameteriv( uint sampler, int pname, int* param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glSamplerParameteriv( uint sampler, int pname, int[] param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glSamplerParameterf( uint sampler, int pname, float param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glSamplerParameterfv( uint sampler, int pname, float* param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glSamplerParameterfv( uint sampler, int pname, float[] param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glSamplerParameterIiv( uint sampler, int pname, int* param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glSamplerParameterIiv( uint sampler, int pname, int[] param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glSamplerParameterIuiv( uint sampler, int pname, uint* param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glSamplerParameterIuiv( uint sampler, int pname, uint[] param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetSamplerParameteriv( uint sampler, int pname, int* param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetSamplerParameteriv( uint sampler, int pname, ref int[] param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetSamplerParameterIiv( uint sampler, int pname, int* param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetSamplerParameterIiv( uint sampler, int pname, ref int[] param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetSamplerParameterfv( uint sampler, int pname, float* param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetSamplerParameterfv( uint sampler, int pname, ref float[] param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetSamplerParameterIuiv( uint sampler, int pname, uint* param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetSamplerParameterIuiv( uint sampler, int pname, ref uint[] param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glQueryCounter( uint id, int target )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetQueryObjecti64v( uint id, int pname, long* param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetQueryObjecti64v( uint id, int pname, ref long[] param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetQueryObjectui64v( uint id, int pname, ulong* param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glGetQueryObjectui64v( uint id, int pname, ref ulong[] param )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttribDivisor( uint index, uint divisor )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttribP1ui( uint index, int type, bool normalized, uint value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribP1uiv( uint index, int type, bool normalized, uint* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribP1uiv( uint index, int type, bool normalized, uint[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttribP2ui( uint index, int type, bool normalized, uint value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribP2uiv( uint index, int type, bool normalized, uint* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribP2uiv( uint index, int type, bool normalized, uint[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttribP3ui( uint index, int type, bool normalized, uint value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribP3uiv( uint index, int type, bool normalized, uint* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribP3uiv( uint index, int type, bool normalized, uint[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void glVertexAttribP4ui( uint index, int type, bool normalized, uint value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribP4uiv( uint index, int type, bool normalized, uint* value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void glVertexAttribP4uiv( uint index, int type, bool normalized, uint[] value )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void Import( GLBindings.GetProcAddressHandler loader )
    {
        Calls++;
        CheckErrors();
    }
}
