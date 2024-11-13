// ///////////////////////////////////////////////////////////////////////////////
// MIT License
// 
// Copyright (c) 2023 dcronqvist (Daniel Cronqvist <daniel@dcronqvist.se>)
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

#pragma warning disable IDE0079    // Remove unnecessary suppression
#pragma warning disable CS8618     // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning disable CS8603     // Possible null reference return.
#pragma warning disable IDE0060    // Remove unused parameter.
#pragma warning disable IDE1006    // Naming Styles.
#pragma warning disable IDE0090    // Use 'new(...)'.
#pragma warning disable CS8500     // This takes the address of, gets the size of, or declares a pointer to a managed type
#pragma warning disable SYSLIB1054 // 

// ----------------------------------------------------------------------------

using Corelib.LibCore.Utils;

using System.Numerics;

using GLenum = System.Int32;
using GLfloat = System.Single;
using GLint = System.Int32;
using GLsizei = System.Int32;
using GLbitfield = System.UInt32;
using GLdouble = System.Double;
using GLuint = System.UInt32;
using GLboolean = System.Boolean;
using GLubyte = System.Byte;
using GLsizeiptr = System.Int32;
using GLintptr = System.Int32;
using GLshort = System.Int16;
using GLbyte = System.SByte;
using GLushort = System.UInt16;
using GLchar = System.Byte;
using GLuint64 = System.UInt64;
using GLint64 = System.Int64;

// ----------------------------------------------------------------------------

namespace Corelib.LibCore.Graphics.OpenGL;

// ----------------------------------------------------------------------------
// ----------------------------------------------------------------------------

/// <summary>
/// Bindings for OpenGL, for core profile. Blazing fast, low level,
/// direct access to the OpenGL API for all versions of OpenGL, using the unmanaged
/// delegates feature in C# 9.0,
/// <para>
/// Also includes a few overloads of many functions to make them a bit more C# friendly
/// (e.g. passing arrays of bytes or floats instead of passing pointers to fixed memory
/// locations). Significant effort has been made to make sure that the overloads are as
/// efficient as possible, in terms of both performance and memory usage.
/// </para>
/// </summary>
[PublicAPI]
public unsafe partial class GLBindings : IGLBindings
{
    public const string LIBGL   = "opengl32.dll";
    public const string LIBGLFW = "glfw3.dll";

    /// <summary>
    /// The null pointer, just like in C/C++.
    /// </summary>
    public readonly void* NULL = ( void* )0;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Returns a Tuple holding the version of OpenGL being used, obtained by
    /// calling <see cref="glGetString"/> with the parameter IGL.GL_VERSION.
    /// </summary>
    public (int major, int minor) GetProjectOpenGLVersion()
    {
        var version = glGetString( IGL.GL_VERSION );

        if ( version == null )
        {
            Logger.Debug( "NULL GL Version returned!" );
        }

        return version == null
            ? ( GraphicsData.DEFAULT_GL_MAJOR, GraphicsData.DEFAULT_GL_MINOR )
            : ( version[ 0 ], version[ 2 ] );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCullFace( GLenum mode )
    {
        _glCullFace( mode );
    }

    [DllImport( LIBGL, EntryPoint = "glCullFace", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCullFace( GLenum mode );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFrontFace( GLenum mode )
    {
        _glFrontFace( mode );
    }

    [DllImport( LIBGL, EntryPoint = "glFrontFace", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glFrontFace( GLenum mode );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glHint( GLenum target, GLenum mode )
    {
        _glHint( target, mode );
    }

    [DllImport( LIBGL, EntryPoint = "glHint", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glHint( GLenum target, GLenum mode );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glLineWidth( GLfloat width )
    {
        _glLineWidth( width );
    }

    [DllImport( LIBGL, EntryPoint = "glLineWidth", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glLineWidth( GLfloat width );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPointSize( GLfloat size )
    {
        _glPointSize( size );
    }

    [DllImport( LIBGL, EntryPoint = "glPointSize", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPointSize( GLfloat size );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPolygonMode( GLenum face, GLenum mode )
    {
        _glPolygonMode( face, mode );
    }

    [DllImport( LIBGL, EntryPoint = "glPolygonMode", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPolygonMode( GLenum face, GLenum mode );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glScissor( GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glScissor( x, y, width, height );
    }

    [DllImport( LIBGL, EntryPoint = "glScissor", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glScissor( GLint x, GLint y, GLsizei width, GLsizei height );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexParameterf( GLenum target, GLenum pname, GLfloat param )
    {
        _glTexParameterf( target, pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glTexParameterf", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexParameterf( GLenum target, GLenum pname, GLfloat param );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexParameterfv( GLenum target, GLenum pname, GLfloat* parameters )
    {
        _glTexParameterfv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glTexParameterfv( GLenum target, GLenum pname, GLfloat[] parameters )
    {
        fixed ( GLfloat* p = &parameters[ 0 ] )
        {
            _glTexParameterfv( target, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexParameterfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexParameterfv( GLenum target, GLenum pname, GLfloat* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexParameteri( GLenum target, GLenum pname, GLint param )
    {
        _glTexParameteri( target, pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glTexParameteri", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexParameteri( GLenum target, GLenum pname, GLint param );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glTexParameteriv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glTexParameteriv( GLenum target, GLenum pname, GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glTexParameteriv( target, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexParameteriv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexParameteriv( GLenum target, GLenum pname, GLint* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexImage1D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, GLenum format, GLenum type, void* pixels )
    {
        _glTexImage1D( target, level, internalformat, width, border, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glTexImage1D< T >( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, GLenum format, GLenum type, T[] pixels )
        where T : unmanaged
    {
        fixed ( void* p = &pixels[ 0 ] )
        {
            _glTexImage1D( target, level, internalformat, width, border, format, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexImage1D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, GLenum format, GLenum type, void* pixels );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexImage2D( GLenum target,
                              GLint level,
                              GLenum internalformat,
                              GLsizei width,
                              GLsizei height,
                              GLint border,
                              GLenum format,
                              GLenum type,
                              void* pixels )
    {
        _glTexImage2D( target, level, internalformat, width, height, border, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glTexImage2D< T >( GLenum target,
                                   GLint level,
                                   GLenum internalformat,
                                   GLsizei width,
                                   GLsizei height,
                                   GLint border,
                                   GLenum format,
                                   GLenum type,
                                   T[] pixels ) where T : unmanaged
    {
        fixed ( void* p = &pixels[ 0 ] )
        {
            _glTexImage2D( target, level, internalformat, width, height, border, format, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexImage2D( GLenum target,
                                              GLint level,
                                              GLenum internalformat,
                                              GLsizei width,
                                              GLsizei height,
                                              GLint border,
                                              GLenum format,
                                              GLenum type,
                                              void* pixels );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawBuffer( GLenum buf )
    {
        _glDrawBuffer( buf );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawBuffer( GLenum buf );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClear( GLbitfield mask )
    {
        _glClear( mask );
    }

    [DllImport( LIBGL, EntryPoint = "glClear", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClear( GLbitfield mask );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha )
    {
        _glClearColor( red, green, blue, alpha );
    }

    [DllImport( LIBGL, EntryPoint = "glClearColor", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClearColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearStencil( GLint s )
    {
        _glClearStencil( s );
    }

    [DllImport( LIBGL, EntryPoint = "glClearStencil", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClearStencil( GLint s );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glClearDepth( GLdouble depth )
    {
        _glClearDepth( depth );
    }

    [DllImport( LIBGL, EntryPoint = "glClearDepth", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glClearDepth( GLdouble depth );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glStencilMask( GLuint mask )
    {
        _glStencilMask( mask );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilMask", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilMask( GLuint mask );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glColorMask( GLboolean red, GLboolean green, GLboolean blue, GLboolean alpha )
    {
        _glColorMask( red, green, blue, alpha );
    }

    [DllImport( LIBGL, EntryPoint = "glColorMask", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glColorMask( GLboolean red, GLboolean green, GLboolean blue, GLboolean alpha );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDepthMask( GLboolean flag )
    {
        _glDepthMask( flag );
    }

    [DllImport( LIBGL, EntryPoint = "glDepthMask", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDepthMask( GLboolean flag );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDisable( GLenum cap )
    {
        _glDisable( cap );
    }

    [DllImport( LIBGL, EntryPoint = "glDisable", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDisable( GLenum cap );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glEnable( GLenum cap )
    {
        _glEnable( cap );
    }

    [DllImport( LIBGL, EntryPoint = "glEnable", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glEnable( GLenum cap );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFinish()
    {
        _glFinish();
    }

    [DllImport( LIBGL, EntryPoint = "glFinish", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glFinish();

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glFlush()
    {
        _glFlush();
    }

    [DllImport( LIBGL, EntryPoint = "glFlush", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glFlush();

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendFunc( GLenum sfactor, GLenum dfactor )
    {
        _glBlendFunc( sfactor, dfactor );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendFunc", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendFunc( GLenum sfactor, GLenum dfactor );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glLogicOp( GLenum opcode )
    {
        _glLogicOp( opcode );
    }

    [DllImport( LIBGL, EntryPoint = "glLogicOp", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glLogicOp( GLenum opcode );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glStencilFunc( GLenum func, GLint @ref, GLuint mask )
    {
        _glStencilFunc( func, @ref, mask );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilFunc", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilFunc( GLenum func, GLint @ref, GLuint mask );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glStencilOp( GLenum fail, GLenum zfail, GLenum zpass )
    {
        _glStencilOp( fail, zfail, zpass );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilOp", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilOp( GLenum fail, GLenum zfail, GLenum zpass );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDepthFunc( GLenum func )
    {
        _glDepthFunc( func );
    }

    [DllImport( LIBGL, EntryPoint = "glDepthFunc", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDepthFunc( GLenum func );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPixelStoref( GLenum pname, GLfloat param )
    {
        _glPixelStoref( pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glPixelStoref", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPixelStoref( GLenum pname, GLfloat param );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPixelStorei( GLenum pname, GLint param )
    {
        _glPixelStorei( pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glPixelStorei", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPixelStorei( GLenum pname, GLint param );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glReadBuffer( GLenum src )
    {
        _glReadBuffer( src );
    }

    [DllImport( LIBGL, EntryPoint = "glReadBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glReadBuffer( GLenum src );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glReadPixels( GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pixels )
    {
        _glReadPixels( x, y, width, height, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glReadPixels< T >( GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, ref T[] pixels ) where T : unmanaged
    {
        fixed ( void* p = &pixels[ 0 ] )
        {
            _glReadPixels( x, y, width, height, format, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glReadPixels", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glReadPixels( GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pixels );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetBooleanv( GLenum pname, GLboolean* data )
    {
        _glGetBooleanv( pname, data );
    }

    /// <inheritdoc/>
    public void glGetBooleanv( GLenum pname, ref GLboolean[] data )
    {
        fixed ( GLboolean* p = &data[ 0 ] )
        {
            _glGetBooleanv( pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetBooleanv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetBooleanv( GLenum pname, GLboolean* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetDoublev( GLenum pname, GLdouble* data )
    {
        _glGetDoublev( pname, data );
    }

    /// <inheritdoc/>
    public void glGetDoublev( GLenum pname, ref GLdouble[] data )
    {
        fixed ( GLdouble* p = &data[ 0 ] )
        {
            _glGetDoublev( pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetDoublev", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetDoublev( GLenum pname, GLdouble* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLenum glGetError()
    {
        return _glGetError();
    }

    [DllImport( LIBGL, EntryPoint = "glGetError", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLenum _glGetError();

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetFloatv( GLenum pname, float* data )
    {
        _glGetFloatv( pname, data );
    }

    /// <inheritdoc/>
    public void glGetFloatv( GLenum pname, ref GLfloat[] data )
    {
        fixed ( GLfloat* p = &data[ 0 ] )
        {
            _glGetFloatv( pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetFloatv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetFloatv( GLenum pname, float* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetIntegerv( GLenum pname, GLint* data )
    {
        _glGetIntegerv( pname, data );
    }

    /// <inheritdoc/>
    public void glGetIntegerv( GLenum pname, ref GLint[] data )
    {
        fixed ( GLint* p = &data[ 0 ] )
        {
            _glGetIntegerv( pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetIntegerv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetIntegerv( GLenum pname, GLint* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLubyte* glGetString( GLenum name )
    {
        return _glGetString( name );
    }

    [DllImport( LIBGL, EntryPoint = "glGetString", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLubyte* _glGetString( GLenum name );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public string glGetStringSafe( GLenum name )
    {
        var p = _glGetString( name );

        if ( p == null )
        {
            return null;
        }

        var i = 0;

        while ( p[ i ] != 0 )
        {
            i++;
        }

        return new string( ( sbyte* )p, 0, i, Encoding.UTF8 );
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTexImage( GLenum target, GLint level, GLenum format, GLenum type, void* pixels )
    {
        _glGetTexImage( target, level, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glGetTexImage< T >( GLenum target, GLint level, GLenum format, GLenum type, ref T[] pixels ) where T : unmanaged
    {
        fixed ( void* p = &pixels[ 0 ] )
        {
            _glGetTexImage( target, level, format, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetIntegerv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetTexImage( GLenum target, GLint level, GLenum format, GLenum type, void* pixels );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTexParameterfv( GLenum target, GLenum pname, GLfloat* parameters )
    {
        _glGetTexParameterfv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetTexParameterfv( GLenum target, GLenum pname, ref GLfloat[] parameters )
    {
        fixed ( GLfloat* p = &parameters[ 0 ] )
        {
            _glGetTexParameterfv( target, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetTexParameterfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetTexParameterfv( GLenum target, GLenum pname, GLfloat* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTexParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetTexParameteriv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetTexParameteriv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetTexParameteriv( target, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetTexParameteriv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetTexParameteriv( GLenum target, GLenum pname, GLint* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTexLevelParameterfv( GLenum target, GLint level, GLenum pname, GLfloat* parameters )
    {
        _glGetTexLevelParameterfv( target, level, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetTexLevelParameterfv( GLenum target, GLint level, GLenum pname, ref GLfloat[] parameters )
    {
        fixed ( GLfloat* p = &parameters[ 0 ] )
        {
            _glGetTexLevelParameterfv( target, level, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetTexLevelParameterfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetTexLevelParameterfv( GLenum target, GLint level, GLenum pname, GLfloat* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetTexLevelParameteriv( GLenum target, GLint level, GLenum pname, GLint* parameters )
    {
        _glGetTexLevelParameteriv( target, level, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetTexLevelParameteriv( GLenum target, GLint level, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetTexLevelParameteriv( target, level, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetTexLevelParameteriv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetTexLevelParameteriv( GLenum target, GLint level, GLenum pname, GLint* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsEnabled( GLenum cap )
    {
        return _glIsEnabled( cap );
    }

    [DllImport( LIBGL, EntryPoint = "glIsEnabled", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsEnabled( GLenum cap );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDepthRange( GLdouble near, GLdouble far )
    {
        _glDepthRange( near, far );
    }

    [DllImport( LIBGL, EntryPoint = "glDepthRange", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDepthRange( GLdouble near, GLdouble far );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glViewport( GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glViewport( x, y, width, height );
    }

    [DllImport( LIBGL, EntryPoint = "glViewport", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glViewport( GLint x, GLint y, GLsizei width, GLsizei height );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawArrays( GLenum mode, GLint first, GLsizei count )
    {
        _glDrawArrays( mode, first, count );
    }

    [DllImport( LIBGL, EntryPoint = "glDrawArrays", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawArrays( GLenum mode, GLint first, GLsizei count );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawElements( GLenum mode, GLsizei count, GLenum type, void* indices )
    {
        _glDrawElements( mode, count, type, indices );
    }

    /// <inheritdoc/>
    public void glDrawElements< T >( GLenum mode, GLsizei count, GLenum type, T[] indices ) where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( T* p = &indices[ 0 ] )
        {
            _glDrawElements( mode, count, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDrawElements", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawElements( GLenum mode, GLsizei count, GLenum type, void* indices );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPolygonOffset( GLfloat factor, GLfloat units )
    {
        _glPolygonOffset( factor, units );
    }

    [DllImport( LIBGL, EntryPoint = "glPolygonOffset", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPolygonOffset( GLfloat factor, GLfloat units );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyTexImage1D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLint border )
    {
        _glCopyTexImage1D( target, level, internalformat, x, y, width, border );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexImage1D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLint border );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyTexImage2D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLsizei height, GLint border )
    {
        _glCopyTexImage2D( target, level, internalformat, x, y, width, height, border );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexImage2D( GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLsizei height, GLint border );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width )
    {
        _glCopyTexSubImage1D( target, level, xoffset, x, y, width );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexSubImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyTexSubImage2D( GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height )
    {
        _glCopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexSubImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexSubImage2D( GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, void* pixels )
    {
        _glTexSubImage1D( target, level, xoffset, width, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glTexSubImage1D< T >( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, T[] pixels )
        where T : unmanaged
    {
        fixed ( void* p = &pixels[ 0 ] )
        {
            _glTexSubImage1D( target, level, xoffset, width, format, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexSubImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, void* pixels );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexSubImage2D( GLenum target,
                                 GLint level,
                                 GLint xoffset,
                                 GLint yoffset,
                                 GLsizei width,
                                 GLsizei height,
                                 GLenum format,
                                 GLenum type,
                                 void* pixels )
    {
        _glTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glTexSubImage2D< T >( GLenum target,
                                      GLint level,
                                      GLint xoffset,
                                      GLint yoffset,
                                      GLsizei width,
                                      GLsizei height,
                                      GLenum format,
                                      GLenum type,
                                      T[] pixels ) where T : unmanaged
    {
        fixed ( void* p = &pixels[ 0 ] )
        {
            _glTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexSubImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexSubImage2D( GLenum target,
                                                 GLint level,
                                                 GLint xoffset,
                                                 GLint yoffset,
                                                 GLsizei width,
                                                 GLsizei height,
                                                 GLenum format,
                                                 GLenum type,
                                                 void* pixels );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindTexture( GLenum target, GLuint texture )
    {
        _glBindTexture( target, texture );
    }

    [DllImport( LIBGL, EntryPoint = "glBindTexture", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindTexture( GLenum target, GLuint texture );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteTextures( GLsizei n, GLuint* textures )
    {
        _glDeleteTextures( n, textures );
    }

    /// <inheritdoc/>
    public void glDeleteTextures( params GLuint[] textures )
    {
        fixed ( void* p = &textures[ 0 ] )
        {
            _glDeleteTextures( textures.Length, ( GLuint* )p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDeleteTextures", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDeleteTextures( GLsizei n, GLuint* textures );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGenTextures( GLsizei n, GLuint* textures )
    {
        _glGenTextures( n, textures );
    }

    /// <inheritdoc/>
    public GLuint[] glGenTextures( GLsizei n )
    {
        var textures = new GLuint[ n ];

        fixed ( void* p = &textures[ 0 ] )
        {
            _glGenTextures( n, ( GLuint* )p );
        }

        return textures;
    }

    /// <inheritdoc/>
    public GLuint glGenTexture()
    {
        return glGenTextures( 1 )[ 0 ];
    }

    [DllImport( LIBGL, EntryPoint = "glGenTextures", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGenTextures( GLsizei n, GLuint* textures );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsTexture( GLuint texture )
    {
        return _glIsTexture( texture );
    }

    [DllImport( LIBGL, EntryPoint = "glGenTextures", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsTexture( GLuint texture );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawRangeElements( GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, void* indices )
    {
        _glDrawRangeElements( mode, start, end, count, type, indices );
    }

    /// <inheritdoc/>
    public void glDrawRangeElements< T >( GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, T[] indices )
        where T : unmanaged, IUnsignedNumber< T >
    {
        fixed ( T* p_indices = &indices[ 0 ] )
        {
            _glDrawRangeElements( mode, start, end, count, type, p_indices );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDrawRangeElements", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawRangeElements( GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, void* indices );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexImage3D( GLenum target,
                              GLint level,
                              GLint internalformat,
                              GLsizei width,
                              GLsizei height,
                              GLsizei depth,
                              GLint border,
                              GLenum format,
                              GLenum type,
                              void* pixels )
    {
        _glTexImage3D( target, level, internalformat, width, height, depth, border, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glTexImage3D< T >( GLenum target,
                                   GLint level,
                                   GLint internalformat,
                                   GLsizei width,
                                   GLsizei height,
                                   GLsizei depth,
                                   GLint border,
                                   GLenum format,
                                   GLenum type,
                                   T[] pixels ) where T : unmanaged
    {
        fixed ( T* p_pixels = &pixels[ 0 ] )
        {
            _glTexImage3D( target, level, internalformat, width, height, depth, border, format, type, p_pixels );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexImage3D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexImage3D( GLenum target,
                                              GLint level,
                                              GLint internalformat,
                                              GLsizei width,
                                              GLsizei height,
                                              GLsizei depth,
                                              GLint border,
                                              GLenum format,
                                              GLenum type,
                                              void* pixels );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glTexSubImage3D( GLenum target,
                                 GLint level,
                                 GLint xoffset,
                                 GLint yoffset,
                                 GLint zoffset,
                                 GLsizei width,
                                 GLsizei height,
                                 GLsizei depth,
                                 GLenum format,
                                 GLenum type,
                                 void* pixels )
    {
        _glTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
    }

    /// <inheritdoc/>
    public void glTexSubImage3D< T >( GLenum target,
                                      GLint level,
                                      GLint xoffset,
                                      GLint yoffset,
                                      GLint zoffset,
                                      GLsizei width,
                                      GLsizei height,
                                      GLsizei depth,
                                      GLenum format,
                                      GLenum type,
                                      T[] pixels ) where T : unmanaged
    {
        fixed ( T* p_pixels = &pixels[ 0 ] )
        {
            _glTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, p_pixels );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glTexSubImage3D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glTexSubImage3D( GLenum target,
                                                 GLint level,
                                                 GLint xoffset,
                                                 GLint yoffset,
                                                 GLint zoffset,
                                                 GLsizei width,
                                                 GLsizei height,
                                                 GLsizei depth,
                                                 GLenum format,
                                                 GLenum type,
                                                 void* pixels );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCopyTexSubImage3D( GLenum target,
                                     GLint level,
                                     GLint xoffset,
                                     GLint yoffset,
                                     GLint zoffset,
                                     GLint x,
                                     GLint y,
                                     GLsizei width,
                                     GLsizei height )
    {
        _glCopyTexSubImage3D( target, level, xoffset, yoffset, zoffset, x, y, width, height );
    }

    [DllImport( LIBGL, EntryPoint = "glCopyTexSubImage3D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCopyTexSubImage3D( GLenum target,
                                                     GLint level,
                                                     GLint xoffset,
                                                     GLint yoffset,
                                                     GLint zoffset,
                                                     GLint x,
                                                     GLint y,
                                                     GLsizei width,
                                                     GLsizei height );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glActiveTexture( GLenum texture )
    {
        _glActiveTexture( texture );
    }

    [DllImport( LIBGL, EntryPoint = "glActiveTexture", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glActiveTexture( GLenum texture );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glSampleCoverage( GLfloat value, GLboolean invert )
    {
        _glSampleCoverage( value, invert );
    }

    [DllImport( LIBGL, EntryPoint = "glSampleCoverage", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glSampleCoverage( GLfloat value, GLboolean invert );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompressedTexImage3D( GLenum target,
                                        GLint level,
                                        GLenum internalformat,
                                        GLsizei width,
                                        GLsizei height,
                                        GLsizei depth,
                                        GLint border,
                                        GLsizei imageSize,
                                        void* data )
    {
        _glCompressedTexImage3D( target, level, internalformat, width, height, depth, border, imageSize, data );
    }

    /// <inheritdoc/>
    public void glCompressedTexImage3D( GLenum target,
                                        GLint level,
                                        GLenum internalformat,
                                        GLsizei width,
                                        GLsizei height,
                                        GLsizei depth,
                                        GLint border,
                                        byte[] data )
    {
        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexImage3D( target, level, internalformat, width, height, depth, border, data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glCompressedTexImage3D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompressedTexImage3D( GLenum target,
                                                        GLint level,
                                                        GLenum internalformat,
                                                        GLsizei width,
                                                        GLsizei height,
                                                        GLsizei depth,
                                                        GLint border,
                                                        GLsizei imageSize,
                                                        void* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompressedTexImage2D( GLenum target,
                                        GLint level,
                                        GLenum internalformat,
                                        GLsizei width,
                                        GLsizei height,
                                        GLint border,
                                        GLsizei imageSize,
                                        void* data )
    {
        _glCompressedTexImage2D( target, level, internalformat, width, height, border, imageSize, data );
    }

    /// <inheritdoc/>
    public void glCompressedTexImage2D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLsizei height, GLint border, byte[] data )
    {
        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexImage2D( target, level, internalformat, width, height, border, data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glCompressedTexImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompressedTexImage2D( GLenum target,
                                                        GLint level,
                                                        GLenum internalformat,
                                                        GLsizei width,
                                                        GLsizei height,
                                                        GLint border,
                                                        GLsizei imageSize,
                                                        void* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompressedTexImage1D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, GLsizei imageSize, void* data )
    {
        _glCompressedTexImage1D( target, level, internalformat, width, border, imageSize, data );
    }

    /// <inheritdoc/>
    public void glCompressedTexImage1D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, byte[] data )
    {
        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexImage1D( target, level, internalformat, width, border, data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glCompressedTexImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompressedTexImage1D( GLenum target, GLint level, GLenum internalformat, GLsizei width, GLint border, GLsizei imageSize, void* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompressedTexSubImage3D( GLenum target,
                                           GLint level,
                                           GLint xoffset,
                                           GLint yoffset,
                                           GLint zoffset,
                                           GLsizei width,
                                           GLsizei height,
                                           GLsizei depth,
                                           GLenum format,
                                           GLsizei imageSize,
                                           void* data )
    {
        _glCompressedTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data );
    }

    /// <inheritdoc/>
    public void glCompressedTexSubImage3D( GLenum target,
                                           GLint level,
                                           GLint xoffset,
                                           GLint yoffset,
                                           GLint zoffset,
                                           GLsizei width,
                                           GLsizei height,
                                           GLsizei depth,
                                           GLenum format,
                                           byte[] data )
    {
        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glCompressedTexSubImage3D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompressedTexSubImage3D( GLenum target,
                                                           GLint level,
                                                           GLint xoffset,
                                                           GLint yoffset,
                                                           GLint zoffset,
                                                           GLsizei width,
                                                           GLsizei height,
                                                           GLsizei depth,
                                                           GLenum format,
                                                           GLsizei imageSize,
                                                           void* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompressedTexSubImage2D( GLenum target,
                                           GLint level,
                                           GLint xoffset,
                                           GLint yoffset,
                                           GLsizei width,
                                           GLsizei height,
                                           GLenum format,
                                           GLsizei imageSize,
                                           void* data )
    {
        _glCompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, imageSize, data );
    }

    /// <inheritdoc/>
    public void glCompressedTexSubImage2D( GLenum target,
                                           GLint level,
                                           GLint xoffset,
                                           GLint yoffset,
                                           GLsizei width,
                                           GLsizei height,
                                           GLenum format,
                                           byte[] data )
    {
        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glCompressedTexSubImage2D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompressedTexSubImage2D( GLenum target,
                                                           GLint level,
                                                           GLint xoffset,
                                                           GLint yoffset,
                                                           GLsizei width,
                                                           GLsizei height,
                                                           GLenum format,
                                                           GLsizei imageSize,
                                                           void* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompressedTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, void* data )
    {
        _glCompressedTexSubImage1D( target, level, xoffset, width, format, imageSize, data );
    }

    /// <inheritdoc/>
    public void glCompressedTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, byte[] data )
    {
        fixed ( byte* p = &data[ 0 ] )
        {
            _glCompressedTexSubImage1D( target, level, xoffset, width, format, data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glCompressedTexSubImage1D", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompressedTexSubImage1D( GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, void* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetCompressedTexImage( GLenum target, GLint level, void* img )
    {
        _glGetCompressedTexImage( target, level, img );
    }

    /// <inheritdoc/>
    public void glGetCompressedTexImage( GLenum target, GLint level, ref byte[] img )
    {
        fixed ( byte* p = &img[ 0 ] )
        {
            _glGetCompressedTexImage( target, level, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetCompressedTexImage", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetCompressedTexImage( GLenum target, GLint level, void* img );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendFuncSeparate( GLenum sfactorRGB, GLenum dfactorRGB, GLenum sfactorAlpha, GLenum dfactorAlpha )
    {
        _glBlendFuncSeparate( sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendFuncSeparate", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendFuncSeparate( GLenum sfactorRGB, GLenum dfactorRGB, GLenum sfactorAlpha, GLenum dfactorAlpha );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glMultiDrawArrays( GLenum mode, GLint* first, GLsizei* count, GLsizei drawcount )
    {
        _glMultiDrawArrays( mode, first, count, drawcount );
    }

    /// <inheritdoc/>
    public void glMultiDrawArrays( GLenum mode, GLint[] first, GLsizei[] count )
    {
        if ( first.Length != count.Length )
        {
            throw new ArgumentException( "first and count arrays must be of the same length" );
        }

        fixed ( GLint* p1 = &first[ 0 ] )
        {
            fixed ( GLsizei* p2 = &count[ 0 ] )
            {
                _glMultiDrawArrays( mode, p1, p2, count.Length );
            }
        }
    }

    [DllImport( LIBGL, EntryPoint = "glMultiDrawArrays", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glMultiDrawArrays( GLenum mode, GLint* first, GLsizei* count, GLsizei drawcount );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glMultiDrawElements( GLenum mode, GLsizei* count, GLenum type, void** indices, GLsizei drawcount )
    {
        _glMultiDrawElements( mode, count, type, indices, drawcount );
    }

    /// <inheritdoc/>
    public void glMultiDrawElements< T >( GLenum mode, GLsizei[] count, GLenum type, T[][] indices ) where T : unmanaged, IUnsignedNumber< T >
    {
        var indexPtrs = new void*[ indices.Length ];

        for ( var i = 0; i < indices.Length; i++ )
        {
            fixed ( void* p = &indices[ i ][ 0 ] )
            {
                indexPtrs[ i ] = p;
            }
        }

        fixed ( GLsizei* c = &count[ 0 ] )
        {
            fixed ( void** p = &indexPtrs[ 0 ] )
            {
                _glMultiDrawElements( mode, c, type, p, count.Length );
            }
        }
    }

    [DllImport( LIBGL, EntryPoint = "glMultiDrawElements", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glMultiDrawElements( GLenum mode, GLsizei* count, GLenum type, void** indices, GLsizei drawcount );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPointParameterf( GLenum pname, GLfloat param )
    {
        _glPointParameterf( pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glPointParameterf", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPointParameterf( GLenum pname, GLfloat param );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPointParameterfv( GLenum pname, GLfloat* parameters )
    {
        _glPointParameterfv( pname, parameters );
    }

    /// <inheritdoc/>
    public void glPointParameterfv( GLenum pname, GLfloat[] parameters )
    {
        fixed ( GLfloat* p = &parameters[ 0 ] )
        {
            _glPointParameterfv( pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glPointParameterfv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPointParameterfv( GLenum pname, GLfloat* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPointParameteri( GLenum pname, GLint param )
    {
        _glPointParameteri( pname, param );
    }

    [DllImport( LIBGL, EntryPoint = "glPointParameteri", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPointParameteri( GLenum pname, GLint param );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glPointParameteriv( GLenum pname, GLint* parameters )
    {
        _glPointParameteriv( pname, parameters );
    }

    /// <inheritdoc/>
    public void glPointParameteriv( GLenum pname, GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glPointParameteriv( pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glPointParameteriv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glPointParameteriv( GLenum pname, GLint* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha )
    {
        _glBlendColor( red, green, blue, alpha );
    }

    [DllImport( LIBGL, EntryPoint = "glBlenColor", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendColor( GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendEquation( GLenum mode )
    {
        _glBlendEquation( mode );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendEquation", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendEquation( GLenum mode );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGenQueries( GLsizei n, GLuint* ids )
    {
        _glGenQueries( n, ids );
    }

    /// <inheritdoc/>
    public GLuint[] glGenQueries( GLsizei n )
    {
        var ret = new GLuint[ n ];

        fixed ( GLuint* p = &ret[ 0 ] )
        {
            _glGenQueries( n, p );
        }

        return ret;
    }

    /// <inheritdoc/>
    public GLuint glGenQuery()
    {
        return glGenQueries( 1 )[ 0 ];
    }

    [DllImport( LIBGL, EntryPoint = "glGenQueries", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGenQueries( GLsizei n, GLuint* ids );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteQueries( GLsizei n, GLuint* ids )
    {
        _glDeleteQueries( n, ids );
    }

    /// <inheritdoc/>
    public void glDeleteQueries( params GLuint[] ids )
    {
        fixed ( GLuint* p = &ids[ 0 ] )
        {
            _glDeleteQueries( ids.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDeleteQueries", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDeleteQueries( GLsizei n, GLuint* ids );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsQuery( GLuint id )
    {
        return _glIsQuery( id );
    }

    [DllImport( LIBGL, EntryPoint = "glIsQuery", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsQuery( GLuint id );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBeginQuery( GLenum target, GLuint id )
    {
        _glBeginQuery( target, id );
    }

    [DllImport( LIBGL, EntryPoint = "glBeginQuery", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBeginQuery( GLenum target, GLuint id );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glEndQuery( GLenum target )
    {
        _glEndQuery( target );
    }

    [DllImport( LIBGL, EntryPoint = "glEndQuery", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glEndQuery( GLenum target );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetQueryiv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetQueryiv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetQueryiv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetQueryiv( target, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetQueryiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetQueryiv( GLenum target, GLenum pname, GLint* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetQueryObjectiv( GLuint id, GLenum pname, GLint* parameters )
    {
        _glGetQueryObjectiv( id, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetQueryObjectiv( GLuint id, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetQueryObjectiv( id, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetQueryObjectiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetQueryObjectiv( GLuint id, GLenum pname, GLint* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetQueryObjectuiv( GLuint id, GLenum pname, GLuint* parameters )
    {
        _glGetQueryObjectuiv( id, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetQueryObjectuiv( GLuint id, GLenum pname, ref GLuint[] parameters )
    {
        fixed ( GLuint* p = &parameters[ 0 ] )
        {
            _glGetQueryObjectuiv( id, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetQueryObjectuiv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetQueryObjectuiv( GLuint id, GLenum pname, GLuint* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindBuffer( GLenum target, GLuint buffer )
    {
        _glBindBuffer( target, buffer );
    }

    [DllImport( LIBGL, EntryPoint = "glBindBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindBuffer( GLenum target, GLuint buffer );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDeleteBuffers( GLsizei n, GLuint* buffers )
    {
        _glDeleteBuffers( n, buffers );
    }

    /// <inheritdoc/>
    public void glDeleteBuffers( params GLuint[] buffers )
    {
        fixed ( GLuint* p = &buffers[ 0 ] )
        {
            _glDeleteBuffers( buffers.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDeleteBuffers", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDeleteBuffers( GLsizei n, GLuint* buffers );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGenBuffers( GLsizei n, GLuint* buffers )
    {
        _glGenBuffers( n, buffers );
    }

    /// <inheritdoc/>
    public GLuint[] glGenBuffers( GLsizei n )
    {
        var ret = new GLuint[ n ];

        fixed ( GLuint* p = &ret[ 0 ] )
        {
            _glGenBuffers( n, p );
        }

        return ret;
    }

    /// <inheritdoc/>
    public GLuint glGenBuffer()
    {
        return glGenBuffers( 1 )[ 0 ];
    }

    [DllImport( LIBGL, EntryPoint = "glGenBuffers", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGenBuffers( GLsizei n, GLuint* buffers );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glIsBuffer( GLuint buffer )
    {
        return _glIsBuffer( buffer );
    }

    [DllImport( LIBGL, EntryPoint = "glIsBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glIsBuffer( GLuint buffer );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBufferData( GLenum target, GLsizeiptr size, void* data, GLenum usage )
    {
        _glBufferData( target, size, data, usage );
    }

    /// <inheritdoc/>
    public void glBufferData< T >( GLenum target, T[] data, GLenum usage ) where T : unmanaged
    {
        fixed ( T* p = &data[ 0 ] )
        {
            _glBufferData( target, sizeof( T ) * data.Length, p, usage );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glBufferData", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBufferData( GLenum target, GLsizeiptr size, void* data, GLenum usage );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBufferSubData( GLenum target, GLintptr offset, GLsizeiptr size, void* data )
    {
        _glBufferSubData( target, offset, size, data );
    }

    /// <inheritdoc/>
    public void glBufferSubData< T >( GLenum target, GLintptr offsetCount, T[] data ) where T : unmanaged
    {
        fixed ( T* p = &data[ 0 ] )
        {
            _glBufferSubData( target, offsetCount * sizeof( T ), sizeof( T ) * data.Length, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glBufferSubData", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBufferSubData( GLenum target, GLintptr offset, GLsizeiptr size, void* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetBufferSubData( GLenum target, GLintptr offset, GLsizeiptr size, void* data )
    {
        _glGetBufferSubData( target, offset, size, data );
    }

    /// <inheritdoc/>
    public void glGetBufferSubData< T >( GLenum target, GLintptr offsetCount, GLsizei count, ref T[] data ) where T : unmanaged
    {
        fixed ( T* p = &data[ 0 ] )
        {
            _glGetBufferSubData( target, offsetCount * sizeof( T ), sizeof( T ) * count, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetBufferSubData", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetBufferSubData( GLenum target, GLintptr offset, GLsizeiptr size, void* data );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void* glMapBuffer( GLenum target, GLenum access )
    {
        return _glMapBuffer( target, access );
    }

    /// <inheritdoc/>
    public Span< T > glMapBuffer< T >( GLenum target, GLenum access ) where T : unmanaged
    {
        GLint size;

        _glGetBufferParameteriv( target, IGL.GL_BUFFER_SIZE, &size );

        var ret = _glMapBuffer( target, access );

        return new Span< T >( ret, size / sizeof( T ) );
    }

    [DllImport( LIBGL, EntryPoint = "glMapBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern void* _glMapBuffer( GLenum target, GLenum access );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLboolean glUnmapBuffer( GLenum target )
    {
        return _glUnmapBuffer( target );
    }

    [DllImport( LIBGL, EntryPoint = "glUnmapBuffer", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLboolean _glUnmapBuffer( GLenum target );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetBufferParameteriv( GLenum target, GLenum pname, GLint* parameters )
    {
        _glGetBufferParameteriv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetBufferParameteriv( GLenum target, GLenum pname, ref GLint[] parameters )
    {
        fixed ( GLint* p = &parameters[ 0 ] )
        {
            _glGetBufferParameteriv( target, pname, p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetBufferParameteriv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetBufferParameteriv( GLenum target, GLenum pname, GLint* parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glGetBufferPointerv( GLenum target, GLenum pname, void** parameters )
    {
        _glGetBufferPointerv( target, pname, parameters );
    }

    /// <inheritdoc/>
    public void glGetBufferPointerv( GLenum target, GLenum pname, ref IntPtr[] parameters )
    {
        fixed ( IntPtr* p = &parameters[ 0 ] )
        {
            _glGetBufferPointerv( target, pname, ( void** )p );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glGetBufferPointerv", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glGetBufferPointerv( GLenum target, GLenum pname, void** parameters );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBlendEquationSeparate( GLenum modeRGB, GLenum modeAlpha )
    {
        _glBlendEquationSeparate( modeRGB, modeAlpha );
    }

    [DllImport( LIBGL, EntryPoint = "glBlendEquationSeparate", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBlendEquationSeparate( GLenum modeRGB, GLenum modeAlpha );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glDrawBuffers( GLsizei n, GLenum* bufs )
    {
        _glDrawBuffers( n, bufs );
    }

    /// <inheritdoc/>
    public void glDrawBuffers( params GLenum[] bufs )
    {
        fixed ( GLenum* pbufs = &bufs[ 0 ] )
        {
            _glDrawBuffers( bufs.Length, pbufs );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glDrawBuffers", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glDrawBuffers( GLsizei n, GLenum* bufs );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glStencilOpSeparate( GLenum face, GLenum sfail, GLenum dpfail, GLenum dppass )
    {
        _glStencilOpSeparate( face, sfail, dpfail, dppass );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilOpSeparate", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilOpSeparate( GLenum face, GLenum sfail, GLenum dpfail, GLenum dppass );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glStencilFuncSeparate( GLenum face, GLenum func, GLint @ref, GLuint mask )
    {
        _glStencilFuncSeparate( face, func, @ref, mask );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilFuncSeparate", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilFuncSeparate( GLenum face, GLenum sfail, GLint @ref, GLuint mask );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glStencilMaskSeparate( GLenum face, GLuint mask )
    {
        _glStencilMaskSeparate( face, mask );
    }

    [DllImport( LIBGL, EntryPoint = "glStencilMaskSeparate", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glStencilMaskSeparate( GLenum face, GLuint mask );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glAttachShader( GLuint program, GLuint shader )
    {
        _glAttachShader( program, shader );
    }

    [DllImport( LIBGL, EntryPoint = "glAttachShader", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glAttachShader( GLuint program, GLuint shader );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glBindAttribLocation( GLuint program, GLuint index, GLchar* name )
    {
        _glBindAttribLocation( program, index, name );
    }

    /// <inheritdoc/>
    public void glBindAttribLocation( GLuint program, GLuint index, string name )
    {
        var utf8 = Encoding.UTF8.GetBytes( name );

        fixed ( byte* putf8 = &utf8[ 0 ] )
        {
            _glBindAttribLocation( program, index, putf8 );
        }
    }

    [DllImport( LIBGL, EntryPoint = "glBindAttribLocation", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glBindAttribLocation( GLuint program, GLuint index, GLchar* name );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public void glCompileShader( GLuint shader )
    {
        _glCompileShader( shader );
    }

    [DllImport( LIBGL, EntryPoint = "glCompileShader", CallingConvention = CallingConvention.Cdecl )]
    private static extern void _glCompileShader( GLuint shader );

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateProgram()
    {
        return _glCreateProgram();
    }

    [DllImport( LIBGL, EntryPoint = "glCreateProgram", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLuint _glCreateProgram();

    // ------------------------------------------------------------------------

    /// <inheritdoc/>
    public GLuint glCreateShader( GLenum type )
    {
        return _glCreateShader( type );
    }

    [DllImport( LIBGL, EntryPoint = "glCreateShader", CallingConvention = CallingConvention.Cdecl )]
    private static extern GLuint _glCreateShader( GLenum type );

    // ------------------------------------------------------------------------
}

#pragma warning restore IDE0079    // Remove unnecessary suppression
#pragma warning restore CS8618     // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning restore CS8603     // Possible null reference return.
#pragma warning restore IDE0060    // Remove unused parameter.
#pragma warning restore IDE1006    // Naming Styles.
#pragma warning restore IDE0090    // Use 'new(...)'.
#pragma warning restore CS8500     // This takes the address of, gets the size of, or declares a pointer to a managed type
#pragma warning restore SYSLIB1054 // 
